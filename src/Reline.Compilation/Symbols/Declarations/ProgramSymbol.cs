namespace Reline.Compilation.Symbols;

public sealed class ProgramSymbol : SymbolNode {

	public IList<LineSymbol> Lines { get; } = new List<LineSymbol>();

}
