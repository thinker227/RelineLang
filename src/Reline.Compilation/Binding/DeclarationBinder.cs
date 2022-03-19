using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

internal partial class Binder {

	/// <summary>
	/// Binds labels, variables and functions from the syntax tree.
	/// </summary>
	private void BindDeclarations() {
		var (labels, variables, functions) = DeclarationBinder.BindDeclarations(this);

		LabelBinder.RegisterRange(labels);
		VariableBinder.RegisterRange(variables);
		FunctionBinder.RegisterRange(functions);
	}

}

internal sealed class DeclarationBinder {

	private readonly IBindingContext context;
	private readonly IdentifierBinder<LabelSymbol> labelBinder;
	private readonly IdentifierBinder<IVariableSymbol> variableBinder;
	private readonly IdentifierBinder<FunctionSymbol> functionBinder;
	private readonly ExpressionEvaluator evaluator;



	private DeclarationBinder(IBindingContext context) {
		this.context = context;
		labelBinder = new();
		variableBinder = new();
		functionBinder = new();
		evaluator = new(context, context);
	}



	public static DeclarationBindResult BindDeclarations(IBindingContext context) {
		DeclarationBinder binder = new(context);
		binder.BindLabels();
		binder.BindVariables();
		binder.BindFunctions();

		return new(
			binder.labelBinder,
			binder.variableBinder,
			binder.functionBinder
		);
	}

	private IIdentifiableSymbol? GetIdentifier(string identifier) =>
		labelBinder.GetSymbol(identifier) ??
		variableBinder.GetSymbol(identifier) ??
		functionBinder.GetSymbol(identifier) ??
		(IIdentifiableSymbol?)null; 

	/// <summary>
	/// Binds all labels from the tree.
	/// </summary>
	/// <remarks>
	/// This does not resolve <see cref="LabelSymbol.Line"/>.
	/// </remarks>
	public void BindLabels() {
		foreach (var lineSyntax in context.SyntaxTree.Root.Lines) {
			if (lineSyntax.Label is null) continue;
			var label = BindLabel(lineSyntax.Label, lineSyntax);
			if (label is not null) labelBinder.RegisterSymbol(label); 
		}
	}
	/// <summary>
	/// Binds the <see cref="LabelSyntax"/> of a <see cref="LineSyntax"/>
	/// into a <see cref="LabelSymbol"/>.
	/// </summary>
	private LabelSymbol? BindLabel(LabelSyntax labelSyntax, LineSyntax lineSyntax) {
		var label = context.GetSymbol<LabelSymbol>(labelSyntax);
		var line = context.GetSymbol<LineSymbol>(lineSyntax);
		label.Identifier = labelSyntax.Identifier.Text;
		label.Line = line;
		line.Label = label;

		string identifier = label.Identifier;
		var existingIdentifier = GetIdentifier(identifier);
		if (existingIdentifier is not null) {
			context.AddDiagnostic(labelSyntax.Identifier, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			return null;
		}

		return label;
	}

	/// <summary>
	/// Binds all variables from assignments.
	/// </summary>
	public void BindVariables() {
		foreach (var line in context.SyntaxTree.Root.Lines) {
			if (line.Statement is not AssignmentStatementSyntax assignment) continue;
			var variable = BindVariableFromLine(assignment, line);
			if (variable is not null) variableBinder.RegisterSymbol(variable);
		}
	}
	/// <summary>
	/// Binds an <see cref="AssignmentStatementSyntax"/> into a <see cref="VariableSymbol"/>.
	/// </summary>
	private VariableSymbol? BindVariableFromLine(AssignmentStatementSyntax syntax, LineSyntax lineSyntax) {
		var assignmentSymbol = context.GetSymbol<AssignmentStatementSymbol>(syntax);
		context.GetSymbol<LineSymbol>(lineSyntax).Statement = assignmentSymbol;

		if (syntax.Identifier.IsMissing) return null;

		string identifier = syntax.Identifier.Text;
		var existingIdentifier = GetIdentifier(identifier);
		if (existingIdentifier is not (null or VariableSymbol)) {
			context.AddDiagnostic(syntax.Identifier, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			return null;
		}

		VariableSymbol symbol = new() {
			Identifier = identifier
		};
		assignmentSymbol.Variable = symbol;
		return symbol;
	}

	/// <summary>
	/// Binds all functions and parameters from the tree.
	/// </summary>
	public void BindFunctions() {
		var funcLines = context.SyntaxTree.Root.Lines
			.Where(l => l.Statement is FunctionDeclarationStatementSyntax)
			.Select(l => (l, (FunctionDeclarationStatementSyntax)l.Statement!))
			.ToArray();

		foreach (var (line, func) in funcLines) {

		}
		foreach (var (line, func) in funcLines) {
			var function = BindFunction(func, line);
			if (function is null) continue;
			functionBinder.RegisterSymbol(function);
			foreach (var param in function.Parameters) variableBinder.RegisterSymbol(param);
		}
	}
	/// <summary>
	/// Binds a function and its parameters.
	/// </summary>
	private FunctionSymbol? BindFunction(FunctionDeclarationStatementSyntax syntax, LineSyntax lineSyntax) {
		var declaration = context.GetSymbol<FunctionDeclarationStatementSymbol>(syntax);
		context.GetSymbol<LineSymbol>(lineSyntax).Statement = declaration;

			// Bind identifier
		// If the identifier is missing, don't bind the function.
		// This causes both the declaration and parameters to have invalid functions.
		FunctionSymbol? function = null;
		if (!syntax.Identifier.IsMissing) {
			function = new();

			string identifier = syntax.Identifier.Text;
			function.Identifier = identifier;
			var existingIdentifier = GetIdentifier(identifier);
			if (existingIdentifier is not null)
				context.AddDiagnostic(syntax.Identifier, CompilerDiagnostics.identifierAlreadyDefined, identifier);

			function.Declaration = declaration;
			declaration.Function = function;
		}

			// Body expression
		// Bind body expression immediately in order to bind parameters to the proper range.
		var expression = ExpressionBinder.BindExpression(syntax.Body, context, ExpressionBindingFlags.ConstantsLabels);
		declaration.RangeExpression = expression;
		var range = evaluator.EvaluateExpression(expression);

		RangeValue rangeValue;
		if (range.Type != BoundValueType.Range) {
			// Reporting a type error when the error is due to an error in a subexpression looks ugly.
			if (range.Type != BoundValueType.None)
				context.AddDiagnostic(expression, CompilerDiagnostics.expectedType, "range");
			// If the range cannot be resolved, set the range to just the line of the declaration.
			int lineNumber = context.SyntaxTree.GetAncestor<LineSyntax>(syntax)!.LineNumber;
			rangeValue = new(lineNumber, lineNumber);
		}
		else rangeValue = range.GetAs<RangeValue>();
		if (function is not null) function.Range = rangeValue;

		// Parameters
		if (syntax.ParameterList is not null) {
			foreach (var p in syntax.ParameterList.Parameters) BindParameter(p, rangeValue, function);
		}

		return function;
	}
	/// <summary>
	/// Binds a <see cref="ParameterSymbol"/> from a
	/// <see cref="SyntaxToken"/> and <see cref="FunctionSymbol"/>.
	/// </summary>
	private ParameterSymbol? BindParameter(SyntaxToken syntax, RangeValue range, FunctionSymbol? function) {
		if (syntax.IsMissing) return null;

		string identifier = syntax.Text;
		ParameterSymbol symbol = new() {
			Identifier = identifier,
			Range = range,
			Function = function
		};

		var existingIdentifier = GetIdentifier(identifier);
		if (existingIdentifier is not null) {
			context.AddDiagnostic(syntax, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			return null;
		}

		function?.Parameters.Add(symbol);
		return symbol;
	}



	/// <summary>
	/// Contains the result of a <see cref="DeclarationBinder"/>.
	/// </summary>
	/// <param name="Labels">The bound labels.</param>
	/// <param name="Variables">The bound variables.</param>
	/// <param name="Functions">The bound functions.</param>
	public readonly record struct DeclarationBindResult(
		IEnumerable<LabelSymbol> Labels,
		IEnumerable<IVariableSymbol> Variables,
		IEnumerable<FunctionSymbol> Functions
	);

}
