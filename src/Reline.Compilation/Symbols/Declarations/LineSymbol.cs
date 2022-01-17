namespace Reline.Compilation.Symbols;

public sealed class LineSymbol : SymbolNode {

	public LabelSymbol? Label { get; set; }
	public IStatementSymbol? Statement { get; set; }
	public int CompileTimeIndex { get; set; }

}
