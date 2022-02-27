using Reline.Compilation.Diagnostics;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

public sealed class SymbolTree {

	public ProgramSymbol Root { get; }
	public ImmutableArray<Diagnostic> Diagnostics { get; }



	internal SymbolTree(ProgramSymbol root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
	}

}
