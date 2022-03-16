using Reline.Compilation.Binding;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a tree of symbol nodes generated by a
/// <see cref="Binding.Binder"/>.
/// </summary>
public sealed class SymbolTree : ISymbolContext {

	private readonly ParentMap<ISymbol> parentMap;



	/// <summary>
	/// The <see cref="Syntax.SyntaxTree"/> which created the tree.
	/// </summary>
	public SyntaxTree SyntaxTree { get; }
	/// <summary>
	/// The root node of the tree.
	/// </summary>
	public ProgramSymbol Root { get; }
	/// <summary>
	/// The diagnostics generated during binding.
	/// </summary>
	public ImmutableArray<Diagnostic> Diagnostics { get; }



	internal SymbolTree(SyntaxTree syntaxTree, ProgramSymbol root, ImmutableArray<Diagnostic> diagnostics) {
		SyntaxTree = syntaxTree;
		Root = root;
		Diagnostics = diagnostics;
		parentMap = new(Root);
	}



	/// <summary>
	/// Binds a <see cref="Syntax.SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	/// <param name="syntaxTree">The <see cref="Syntax.SyntaxTree"/> to bind.</param>
	/// <returns>A new <see cref="SymbolTree"/>.</returns>
	public static SymbolTree BindTree(SyntaxTree syntaxTree) =>
		Binder.BindTree(syntaxTree);

	/// <summary>
	/// Gets the parent node of a specified <see cref="ISymbol"/>.
	/// </summary>
	/// <param name="symbol">The <see cref="ISymbol"/>
	/// to get the parent of.</param>
	/// <returns>The parent of <paramref name="symbol"/>, or <see langword="null"/>
	/// if the node is the root of the symbol tree.</returns>
	public ISymbol? GetParent(ISymbol symbol) =>
		parentMap.GetParent(symbol);

}
