namespace Reline.Compilation.Symbols;

public sealed class ProgramSymbol : SymbolNode {

	public IList<LineSymbol> Lines { get; } = new List<LineSymbol>();
	public int StartLine { get; set; }
	public int EndLine { get; set; }

}
