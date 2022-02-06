using Reline.Compilation.Diagnostics;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

public sealed class SymbolTree {

	public ImmutableArray<Diagnostic> Diagnostics { get; }
	public ProgramSymbol Root { get; }



	public SymbolTree(ProgramSymbol root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
	}

}
