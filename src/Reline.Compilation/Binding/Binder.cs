using Reline.Compilation.Diagnostics;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

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
	internal void AddDiagnostic(ISymbol symbol, DiagnosticDescription description, params object?[] formatArgs) {
		var textSpan = symbol.Syntax?.GetTextSpan() ?? TextSpan.Empty;
		var diagnostic = description
			.ToDiagnostic(textSpan, formatArgs);
		diagnostics.AddDiagnostic(symbol, diagnostic);

		if (description.Level == DiagnosticLevel.Error) hasError = true;
	}
	/// <summary>
	/// Gets a symbol corresponding to an identifier.
	/// </summary>
	/// <param name="identifier">The identifier to get the symbol of.</param>
	/// <returns>A <see cref="IIdentifiableSymbol"/> corresponding to
	/// <paramref name="identifier"/>, or <see langword="null"/> if none was found.</returns>
	internal IIdentifiableSymbol? GetIdentifier(string identifier) {
		var label = labelBinder.GetSymbol(identifier);
		if (label is not null) return label;

		var variable = variableBinder.GetSymbol(identifier);
		if (variable is not null) return variable;

		var function = functionBinder.GetSymbol(identifier);
		if (function is not null) return function;

		return null;
	}

}
