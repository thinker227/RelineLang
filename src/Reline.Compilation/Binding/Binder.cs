using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds syntax nodes into symbols.
/// </summary>
public sealed partial class Binder : ISymbolContext {

	private readonly BinderDiagnosticMap diagnostics;
	private readonly SyntaxSymbolBinder syntaxSymbolBinder;
	private ParentMap<ISymbol>? symbolParentMap;
	private ProgramSymbol? programRoot;
	private bool hasError;

	/// <summary>
	/// The <see cref="Parsing.SyntaxTree"/> being bound.
	/// </summary>
	internal SyntaxTree SyntaxTree { get; }
	/// <summary>
	/// The <see cref="ProgramSymbol"/> which is the root of the symbol tree.
	/// </summary>
	internal ProgramSymbol ProgramRoot =>
		programRoot ??
		throw new InvalidOperationException("Program root uninitialized.");
	ProgramSymbol ISymbolContext.Root => ProgramRoot;
	/// <summary>
	/// The internal <see cref="LabelSymbol"/> binder.
	/// </summary>
	internal IdentifierBinder<LabelSymbol> LabelBinder { get; }
	/// <summary>
	/// The internal <see cref="IVariableSymbol"/> binder.
	/// </summary>
	internal IdentifierBinder<IVariableSymbol> VariableBinder { get; }
	/// <summary>
	/// The internal <see cref="FunctionSymbol"/> binder.
	/// </summary>
	internal IdentifierBinder<FunctionSymbol> FunctionBinder { get; }
	/// <summary>
	/// The <see cref="Binding.ExpressionEvaluator"/> for the binder.
	/// </summary>
	internal ExpressionEvaluator ExpressionEvaluator { get; }
	/// <summary>
	/// Whether any errors have been generated.
	/// </summary>
	internal bool HasError => hasError;



	private Binder(SyntaxTree tree) {
		diagnostics = new();
		syntaxSymbolBinder = new();
		symbolParentMap = null;
		hasError = false;
		programRoot = null;

		SyntaxTree = tree;
		LabelBinder = new();
		VariableBinder = new();
		FunctionBinder = new();
		ExpressionEvaluator = new(this);
	}



	/// <summary>
	/// Binds a <see cref="Parsing.SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	/// <param name="tree">The syntax tree to bind.</param>
	/// <returns>An <see cref="IOperationResult{T}"/>
	/// containing the bound <see cref="SymbolTree"/>.</returns>
	public static SymbolTree BindTree(SyntaxTree tree) {
		Binder binder = new(tree);
		var result = binder.BindTree();
		return result;
	}
	/// <summary>
	/// Binds a <see cref="Parsing.SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	private SymbolTree BindTree() {
		programRoot = BindProgramPartialFromTree();
		symbolParentMap = new(ProgramRoot);
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
	internal IIdentifiableSymbol? GetIdentifier(string identifier) =>
		LabelBinder.GetSymbol(identifier) ??
		VariableBinder.GetSymbol(identifier) ??
		FunctionBinder.GetSymbol(identifier) ??
		(IIdentifiableSymbol?)null;

	internal ISymbol? GetParent(ISymbol symbol) =>
		(symbolParentMap ?? throw new InvalidOperationException("Parent map uninitialized."))
		.GetParent(symbol);
	ISymbol? ISymbolContext.GetParent(ISymbol node) =>
		GetParent(node);

}
