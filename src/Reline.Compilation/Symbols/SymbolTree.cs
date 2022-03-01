using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a tree of symbol nodes generated by a
/// <see cref="Binding.Binder"/>.
/// </summary>
public sealed class SymbolTree {

	private readonly ParentMap<ISymbol> parentMap;



	/// <summary>
	/// The root node of the tree.
	/// </summary>
	public ProgramSymbol Root { get; }
	/// <summary>
	/// The diagnostics generated during binding.
	/// </summary>
	public ImmutableArray<Diagnostic> Diagnostics { get; }



	internal SymbolTree(ProgramSymbol root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
		parentMap = new(Root);
	}



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
