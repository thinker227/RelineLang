using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds syntax nodes into symbols.
/// </summary>
internal sealed partial class Binder : IBindingContext {

	private readonly List<Diagnostic> diagnostics;
	private readonly SyntaxSymbolBinder syntaxSymbolBinder;
	private ParentMap<ISymbol>? symbolParentMap;
	private ProgramSymbol? programRoot;
	private bool hasError;

	/// <summary>
	/// The <see cref="Parsing.SyntaxTree"/> being bound.
	/// </summary>
	public SyntaxTree SyntaxTree { get; }
	/// <summary>
	/// The <see cref="ProgramSymbol"/> which is the root of the symbol tree.
	/// </summary>
	public ProgramSymbol ProgramRoot =>
		programRoot ??
		throw new InvalidOperationException("Program root uninitialized.");
	ProgramSymbol ISemanticContext.Root => ProgramRoot;
	/// <summary>
	/// The internal <see cref="LabelSymbol"/> binder.
	/// </summary>
	public IdentifierBinder<LabelSymbol> LabelBinder { get; }
	/// <summary>
	/// The internal <see cref="IVariableSymbol"/> binder.
	/// </summary>
	public IdentifierBinder<IVariableSymbol> VariableBinder { get; }
	/// <summary>
	/// The internal <see cref="FunctionSymbol"/> binder.
	/// </summary>
	public FunctionBinder FunctionBinder { get; }
	/// <summary>
	/// The <see cref="Binding.ExpressionEvaluator"/> for the binder.
	/// </summary>
	public ExpressionEvaluator ExpressionEvaluator { get; }
	/// <summary>
	/// The <see cref="SymbolFactory"/> used to create symbols.
	/// </summary>
	public SymbolFactory Factory { get; }
	/// <summary>
	/// Whether any errors have been generated.
	/// </summary>
	public bool HasError => hasError;



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
		ExpressionEvaluator = new(this, this);
		Factory = new(this);
	}



	/// <summary>
	/// Binds a <see cref="Parsing.SyntaxTree"/> into a <see cref="SemanticModel"/>.
	/// </summary>
	/// <param name="tree">The syntax tree to bind.</param>
	/// <returns>An <see cref="IOperationResult{T}"/>
	/// containing the bound <see cref="SemanticModel"/>.</returns>
	public static SemanticModel BindTree(SyntaxTree tree) {
		Binder binder = new(tree);
		var result = binder.BindTree();
		return result;
	}
	/// <summary>
	/// Binds a <see cref="Parsing.SyntaxTree"/> into a <see cref="SemanticModel"/>.
	/// </summary>
	private SemanticModel BindTree() {
		programRoot = BindProgramPartialFromTree();
		symbolParentMap = new(ProgramRoot);

		var declarations = DeclarationBinder.BindDeclarations(this);
		LabelBinder.RegisterRange(declarations.Labels);
		VariableBinder.RegisterRange(declarations.Variables);
		FunctionBinder.RegisterRange(declarations.Functions);

		BindProgram(ProgramRoot);
		
		var diagnostics = this.diagnostics.ToImmutableArray();
		return new(
			SyntaxTree,
			ProgramRoot,
			diagnostics,
			LabelBinder.ToImmutableArray(),
			VariableBinder.ToImmutableArray(),
			FunctionBinder.ToImmutableArray()
		);
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
	public TSymbol GetSymbol<TSyntax, TSymbol>(TSyntax syntax, Func<TSyntax, TSymbol> factory) where TSyntax : ISyntaxNode where TSymbol : ISymbol {
		if (syntaxSymbolBinder.TryGetSymbol(syntax, out var bound))
			return (TSymbol)bound;

		var symbol = factory(syntax);
		syntaxSymbolBinder.Bind(syntax, symbol);
		return symbol;
	}
	private TSymbol GetSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new() =>
		BindingContextExtensions.GetSymbol<TSymbol>(this, syntax);

	/// <summary>
	/// Adds a diagnostic.
	/// </summary>
	/// <param name="location">The location of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public void AddDiagnostic(TextSpan? location, DiagnosticDescription description, params object?[] formatArgs) {
		var diagnostic = Diagnostic.Create(description, location, formatArgs);
		diagnostics.Add(diagnostic);

		if (description.Level == DiagnosticLevel.Error) hasError = true;
	}

	/// <summary>
	/// Gets a symbol corresponding to an identifier.
	/// </summary>
	/// <param name="identifier">The identifier to get the symbol of.</param>
	/// <returns>A <see cref="IIdentifiableSymbol"/> corresponding to
	/// <paramref name="identifier"/>, or <see langword="null"/> if none was found.</returns>
	public IIdentifiableSymbol? GetIdentifier(string identifier) =>
		LabelBinder.GetSymbol(identifier) ??
		VariableBinder.GetSymbol(identifier) ??
		FunctionBinder.GetSymbol(identifier) ??
		(IIdentifiableSymbol?)null;

	/// <summary>
	/// Gets the parent node of a specified <see cref="ISymbol"/>.
	/// </summary>
	/// <param name="symbol">The <see cref="ISymbol"/>
	/// to get the parent of.</param>
	/// <returns>The parent of <paramref name="symbol"/>, or <see langword="null"/>
	/// if the node is the root of the context.</returns>
	public ISymbol? GetParent(ISymbol symbol) =>
		(symbolParentMap ?? throw new InvalidOperationException("Parent map uninitialized."))
		.GetParent(symbol);

	/// <summary>
	/// Binds a <see cref="IExpressionSyntax"/> into a <see cref="IExpressionSymbol"/>
	/// using the current <see cref="Binder"/> as the context.
	/// </summary>
	/// <param name="syntax">The syntax to bind.</param>
	/// <param name="flags">The <see cref="ExpressionBindingFlags"/>
	/// to use to determine what is permitted in the expression.</param>
	/// <returns></returns>
	public IExpressionSymbol BindExpression(
		IExpressionSyntax syntax,
		ExpressionBindingFlags flags = ExpressionBindingFlags.None
	) => ExpressionBinder.BindExpression(syntax, this, flags);

}
