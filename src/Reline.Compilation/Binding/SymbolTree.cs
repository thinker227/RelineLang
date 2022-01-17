using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

public sealed class SymbolTree {

	public ProgramSymbol Root { get; }



	public SymbolTree(ProgramSymbol root) {
		Root = root;
	}

}
