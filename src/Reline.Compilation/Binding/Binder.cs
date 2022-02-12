using Reline.Compilation.Diagnostics;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds syntax nodes into symbols.
/// </summary>
public sealed partial class Binder {

	private readonly BinderDiagnosticMap diagnostics;
	private readonly SyntaxSymbolBinder syntaxSymbolBinder;
	private ProgramSymbol programRoot;
	private bool hasError;

	/// <summary>
	/// The <see cref="Parsing.SyntaxTree"/> being bound.
	/// </summary>
	internal SyntaxTree SyntaxTree { get; }
	/// <summary>
	/// The <see cref="ProgramSymbol"/> which is the root of the symbol tree.
	/// </summary>
	internal ProgramSymbol ProgramRoot => programRoot;
	/// <summary>
	/// The <see cref="Binding.LabelBinder"/> for the binder.
	/// </summary>
	internal LabelBinder LabelBinder { get; }
	/// <summary>
	/// The <see cref="Binding.VariableBinder"/> for the binder.
	/// </summary>
	internal VariableBinder VariableBinder { get; }
	/// <summary>
	/// The <see cref="Binding.FunctionBinder"/> for the binder.
	/// </summary>
	internal FunctionBinder FunctionBinder { get; }
	/// <summary>
	/// The <see cref="Binding.ExpressionEvaluator"/> for the binder.
	/// </summary>
	internal ExpressionEvaluator ExpressionEvaluator { get; }
	/// <summary>
	/// The current <see cref="LineSymbol"/> being bound.
	/// </summary>
	internal LineSymbol? CurrentLine { get; set; }
	/// <summary>
	/// Whether any errors have been generated.
	/// </summary>
	internal bool HasError => hasError;



	private Binder(SyntaxTree tree) {
		diagnostics = new();
		syntaxSymbolBinder = new();
		hasError = false;
		programRoot = null!;

		SyntaxTree = tree;
		LabelBinder = new();
		VariableBinder = new();
		FunctionBinder = new();
		ExpressionEvaluator = new(this);
		CurrentLine = null;
	}



	/// <summary>
	/// Binds a <see cref="Parsing.SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	/// <param name="tree">The syntax tree to bind.</param>
	/// <returns>An <see cref="IOperationResult{T}"/>
	/// containing the bound <see cref="SymbolTree"/>.</returns>
	public static SymbolTree BindTree(SyntaxTree tree) {
		Binder symbolCompiler = new(tree);
		var result = symbolCompiler.BindTree();
		return result;
	}
	/// <summary>
	/// Binds a <see cref="Parsing.SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	private SymbolTree BindTree() {
		programRoot = BindProgramPartialFromTree();
		BindLabelsFromTree();
		BindVariablesFromAssignments();
		BindFunctionsFromTree();
		BindProgram(ProgramRoot);

		var diagnostics = this.diagnostics.ToImmutableArray();
		return new(ProgramRoot, diagnostics);
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
		var textSpan = symbol.Syntax?.GetTextSpan();
		Diagnostic diagnostic = new() {
			Level = level,
			Description = description,
			Location = textSpan
		};
		diagnostics.AddDiagnostic(symbol, diagnostic);

		if (level == DiagnosticLevel.Error) hasError = true;
	}

	/// <summary>
	/// Gets a symbol corresponding to an identifier.
	/// </summary>
	/// <param name="identifier">The identifier to get the symbol of.</param>
	/// <returns>A <see cref="IIdentifiableSymbol"/> corresponding to
	/// <paramref name="identifier"/>, or <see langword="null"/> if none was found.</returns>
	internal IIdentifiableSymbol? GetIdentifier(string identifier) {
		var label = LabelBinder.GetSymbol(identifier);
		if (label is not null) return label;

		var variable = VariableBinder.GetSymbol(identifier);
		if (variable is not null) return variable;

		var function = FunctionBinder.GetSymbol(identifier);
		if (function is not null) return function;

		return null;
	}

}
