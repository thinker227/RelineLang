using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Symbols;

public sealed class SymbolTree {

	private readonly ParentMap<ISymbol> parentMap;



	public ProgramSymbol Root { get; }
	public ImmutableArray<Diagnostic> Diagnostics { get; }



	internal SymbolTree(ProgramSymbol root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
		parentMap = new(Root);
	}

	public ISymbol? GetParent(ISymbol symbol) =>
		parentMap.GetParent(symbol);

}
