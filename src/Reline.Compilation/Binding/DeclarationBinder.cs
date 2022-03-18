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
		BindLabelsFromTree();
		BindVariablesFromTree();
		BindFunctionsFromTree();
	}

	/// <summary>
	/// Binds all labels from the tree.
	/// </summary>
	/// <remarks>
	/// This does not resolve <see cref="LabelSymbol.Line"/>.
	/// </remarks>
	private void BindLabelsFromTree() {
		foreach (var lineSyntax in SyntaxTree.Root.Lines) BindLabelFromLine(lineSyntax);
	}
	/// <summary>
	/// Binds the <see cref="LabelSyntax"/> of a <see cref="LineSyntax"/>
	/// into a <see cref="LabelSymbol"/>.
	/// </summary>
	private void BindLabelFromLine(LineSyntax lineSyntax) {
		var labelSyntax = lineSyntax.Label;
		if (labelSyntax is null) return;

		var label = GetSymbol<LabelSymbol>(labelSyntax);
		var line = BindLinePartialFromLabel(lineSyntax, label);
		label.Identifier = labelSyntax.Identifier.Text;
		label.Line = line;
		line.Label = label;

		string identifier = label.Identifier;
		var existingIdentifier = GetIdentifier(identifier);
		if (existingIdentifier is not null) {
			AddDiagnostic(labelSyntax.Identifier, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			return;
		}

		LabelBinder.RegisterSymbol(label);
		ProgramRoot.Labels.Add(label);
	}
	/// <summary>
	/// Partially binds a <see cref="LabelSyntax"/> into a <see cref="LineSymbol"/>
	/// from a <see cref="LabelSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="LineSymbol.LineNumber"/> and <see cref="LineSymbol.Label"/>.
	/// </remarks>
	private LineSymbol BindLinePartialFromLabel(LineSyntax syntax, LabelSymbol label) {
		var symbol = BindLinePartial(syntax);
		symbol.Label = label;
		return symbol;
	}

	/// <summary>
	/// Binds all variables from assignments.
	/// </summary>
	private void BindVariablesFromTree() {
		foreach (var line in SyntaxTree.Root.Lines) BindVariableFromLine(line);
	}
	/// <summary>
	/// Binds an <see cref="AssignmentStatementSyntax"/> into a <see cref="VariableSymbol"/>.
	/// </summary>
	private void BindVariableFromLine(LineSyntax lineSyntax) {
		if (lineSyntax.Statement is not AssignmentStatementSyntax syntax) return;

		var assignmentSymbol = GetSymbol<AssignmentStatementSymbol>(syntax);
		GetSymbol<LineSymbol>(lineSyntax).Statement = assignmentSymbol;

		if (syntax.Identifier.IsMissing) return;

		string identifier = syntax.Identifier.Text;
		VariableSymbol symbol = new() {
			Identifier = syntax.Identifier.Text
		};
		var existingIdentifier = GetIdentifier(identifier);
		if (existingIdentifier is not (null or VariableSymbol)) {
			AddDiagnostic(syntax.Identifier, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			return;
		}

		assignmentSymbol.Variable = symbol;
		VariableBinder.RegisterSymbol(symbol);
		ProgramRoot.Variables.Add(symbol);
	}

	/// <summary>
	/// Binds all functions and parameters from the tree.
	/// </summary>
	private void BindFunctionsFromTree() {
		var funcLines = SyntaxTree.Root.Lines
			.Where(l => l.Statement is FunctionDeclarationStatementSyntax)
			.Select(l => (l, (FunctionDeclarationStatementSyntax)l.Statement!))
			.ToArray();

		foreach (var (line, func) in funcLines) {
			BindFunction(func, line);
		}
	}
	/// <summary>
	/// Binds a function and its parameters.
	/// </summary>
	private void BindFunction(FunctionDeclarationStatementSyntax syntax, LineSyntax lineSyntax) {
		var declaration = GetSymbol<FunctionDeclarationStatementSymbol>(syntax);
		GetSymbol<LineSymbol>(lineSyntax).Statement = declaration;

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
				AddDiagnostic(syntax.Identifier, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			else {
				FunctionBinder.RegisterSymbol(function);
				ProgramRoot.Functions.Add(function);
			}

			function.Declaration = declaration;
			declaration.Function = function;
		}

			// Body expression
		// Bind body expression immediately in order to bind parameters to the proper range.
		var expression = BindExpression(syntax.Body, ExpressionBindingFlags.ConstantsLabels);
		declaration.RangeExpression = expression;
		var range = ExpressionEvaluator.EvaluateExpression(expression);
		RangeValue rangeValue;
		if (range.Type != BoundValueType.Range) {
			// Reporting a type error when the error is due to an error in a subexpression looks ugly.
			if (range.Type != BoundValueType.None)
				AddDiagnostic(expression, CompilerDiagnostics.expectedType, "range");
			// If the range cannot be resolved, set the range to just the line of the declaration.
			int lineNumber = SyntaxTree.GetAncestor<LineSyntax>(syntax)!.LineNumber;
			rangeValue = new(lineNumber, lineNumber);
		}
		else rangeValue = range.GetAs<RangeValue>();
		if (function is not null) function.Range = rangeValue;

		// Parameters
		if (syntax.ParameterList is not null) {
			foreach (var p in syntax.ParameterList.Parameters) BindParameter(p, rangeValue, function);
		}
	}
	/// <summary>
	/// Binds a <see cref="ParameterSymbol"/> from a
	/// <see cref="SyntaxToken"/> and <see cref="FunctionSymbol"/>.
	/// </summary>
	private void BindParameter(SyntaxToken syntax, RangeValue range, FunctionSymbol? function) {
		if (syntax.IsMissing) return;

		string identifier = syntax.Text;
		ParameterSymbol symbol = new() {
			Identifier = identifier,
			Range = range,
			Function = function
		};

		var existingIdentifier = GetIdentifier(identifier);
		if (existingIdentifier is not null) {
			AddDiagnostic(syntax, CompilerDiagnostics.identifierAlreadyDefined, identifier);
			return;
		}

		function?.Parameters.Add(symbol);
		VariableBinder.RegisterSymbol(symbol);
		ProgramRoot.Variables.Add(symbol);
	}

}
