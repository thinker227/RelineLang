using Reline.Compilation.Diagnostics;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using VType = Reline.Compilation.Symbols.ValueType;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds syntax nodes into symbols.
/// </summary>
public sealed partial class Binder {

	internal readonly SyntaxTree tree;
	internal readonly BinderDiagnosticMap diagnostics;
	internal readonly SyntaxSymbolBinder syntaxSymbolBinder;
	internal readonly LabelBinder labelBinder;
	internal readonly VariableBinder variableBinder;
	internal readonly FunctionBinder functionBinder;
	internal readonly ConstantExpressionEvaluator expressionEvaluator;
	internal bool hasError;



	private Binder(SyntaxTree tree) {
		this.tree = tree;
		diagnostics = new();
		syntaxSymbolBinder = new();
		labelBinder = new();
		variableBinder = new();
		functionBinder = new();
		expressionEvaluator = new(this);
		hasError = false;
	}



	/// <summary>
	/// Binds a <see cref="SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	/// <param name="tree">The syntax tree to bind.</param>
	/// <returns>An <see cref="IOperationResult{T}"/>
	/// containing the bound <see cref="SymbolTree"/>.</returns>
	public static IOperationResult<SymbolTree> BindTree(SyntaxTree tree) {
		Binder symbolCompiler = new(tree);
		var result = symbolCompiler.BindTree();
		return new SymbolCompilationResult(ImmutableArray<Diagnostic>.Empty, result);
	}
	/// <summary>
	/// Binds a <see cref="SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	private SymbolTree BindTree() {
		BindLabelsFromTree();
		BindVariablesFromAssignments();
		BindFunctionsFromTree();
		var program = BindProgram(tree.Root);
		return new(program);
	}

	/// <summary>
	/// Binds all labels from the tree.
	/// </summary>
	/// <remarks>
	/// This does not resolve <see cref="LabelSymbol.Line"/>.
	/// </remarks>
	private void BindLabelsFromTree() {
		var labels = tree.GetLabels();
		foreach (var label in labels) {
			var symbol = CreateSymbol<LabelSymbol>(label);
			symbol.Identifier = label.Identifier.Text;
			labelBinder.RegisterSymbol(symbol);
		}
	}
	/// <summary>
	/// Binds all variables from assignments.
	/// </summary>
	private void BindVariablesFromAssignments() {
		var assignments = tree.GetStatementsOfType<AssignmentStatementSyntax>();
		foreach (var assignment in assignments) {
			VariableSymbol symbol = new() {
				Identifier = assignment.Identifier.Text
			};
			variableBinder.RegisterSymbol(symbol);
		}
	}
	/// <summary>
	/// Binds all functions and parameters from the tree.
	/// </summary>
	/// <remarks>
	/// This does not resolve
	/// <see cref="FunctionSymbol.Parameters"/>,
	/// <see cref="FunctionSymbol.Range"/> or
	/// <see cref="FunctionSymbol.Body"/>.
	/// </remarks>
	private void BindFunctionsFromTree() {
		var functions = tree.GetStatementsOfType<FunctionDeclarationStatementSyntax>();
		foreach (var function in functions) BindFunction(function);
	}
	/// <summary>
	/// Binds a function and its parameters.
	/// </summary>
	private void BindFunction(FunctionDeclarationStatementSyntax function) {
		var symbol = CreateSymbol<FunctionSymbol>(function);
		symbol.Identifier = function.Identifier.Text;
		// Bind body expression immediately in order to bind parameters to the proper range
		symbol.Range = BindExpression(function.Body, ExpressionBindingFlags.ConstantsLabels, VType.Range);
		functionBinder.RegisterSymbol(symbol);
		
		if (symbol.Range.GetValueType() != VType.Range || function.ParameterList is null) return;

		foreach (var p in function.ParameterList.Parameters) {
			var range = expressionEvaluator.EvaluateExpression(symbol.Range).GetAs<RangeLiteral>();
			ParameterSymbol parameter = new() {
				Identifier = p.Text,
				Range = range,
				Function = symbol
			};
			variableBinder.RegisterSymbol(parameter);
		}
	}

	/// <summary>
	/// Binds a <see cref="ProgramSyntax"/>
	/// into a <see cref="ProgramSymbol"/>.
	/// </summary>
	/// <param name="syntax"></param>
	/// <returns></returns>
	private ProgramSymbol BindProgram(ProgramSyntax syntax) {
		var symbol = CreateSymbol<ProgramSymbol>(syntax);

		foreach (var l in syntax.Lines) {
			symbol.Lines.Add(BindLine(l));
		}

		return symbol;
	}
	/// <summary>
	/// Binds a <see cref="LineSyntax"/>
	/// into a <see cref="LineSymbol"/>.
	/// </summary>
	/// <param name="syntax"></param>
	/// <returns></returns>
	private LineSymbol BindLine(LineSyntax syntax) {
		var symbol = CreateSymbol<LineSymbol>(syntax);

		if (syntax.Label is not null && !syntax.Label.Identifier.IsMissing) {
			symbol.Label = BindLabel(syntax.Label, symbol);
		}

		if (syntax.Statement is not null) {
			symbol.Statement = BindStatement(syntax.Statement);
		}

		return symbol;
	}
	/// <summary>
	/// Binds a <see cref="LabelSyntax"/> into a <see cref="LabelSymbol"/>.
	/// </summary>
	private LabelSymbol BindLabel(LabelSyntax syntax, LineSymbol line) {
		var symbol = CreateSymbol<LabelSymbol>(syntax);
		var identifier = syntax.Identifier.Text;
		symbol.Identifier = identifier;
		symbol.Line = line;
		return symbol;
	}

	/// <summary>
	/// If <paramref name="syntax"/> is not present in <see cref="syntaxSymbolBinder"/>,
	/// creates a symbol with <see cref="ISymbol.Syntax"/> set to <paramref name="syntax"/>
	/// and the symbol registered in <see cref="syntaxSymbolBinder"/>
	/// if <paramref name="syntax"/> is not <see langword="null"/>.
	/// Otherwise, returns the <see cref="ISymbol"/> in <see cref="syntaxSymbolBinder"/>.
	/// </summary>
	/// <typeparam name="TSymbol">The type of the symbol to create.</typeparam>
	/// <param name="syntax">The syntax of the symbol.</param>
	internal TSymbol CreateSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new() {
		if (syntaxSymbolBinder.TryGetSymbol(syntax, out var bound))
			return (TSymbol)bound;

		TSymbol symbol = new() { Syntax = syntax };
		syntaxSymbolBinder.Bind(syntax, symbol);
		return symbol;
	}
	/// <summary>
	/// Adds a diagnostic to a symbol.
	/// </summary>
	/// <param name="symbol">The to add the diagnostic to.</param>
	/// <param name="level">The <see cref="DiagnosticLevel"/> of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	internal void AddDiagnostic(ISymbol symbol, DiagnosticLevel level, string description) {
		var textSpan = symbol.Syntax?.GetTextSpan() ?? TextSpan.Empty;
		Diagnostic diagnostic = new() {
			Level = level,
			Description = description,
			Location = textSpan
		};
		diagnostics.AddDiagnostic(symbol, diagnostic);

		if (level == DiagnosticLevel.Error) hasError = true;
	}

}
