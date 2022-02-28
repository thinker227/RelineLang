using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Symbols;

public sealed class SymbolTree {

	public ProgramSymbol Root { get; }
	public ImmutableArray<Diagnostic> Diagnostics { get; }



	internal SymbolTree(ProgramSymbol root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
	}

}
