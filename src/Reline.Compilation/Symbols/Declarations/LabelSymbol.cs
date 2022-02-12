namespace Reline.Compilation.Symbols;

public sealed class LabelSymbol : SymbolNode, IIdentifiableSymbol {

	public string Identifier { get; set; } = null!;
	public LineSymbol Line { get; set; } = null!;
	public IList<ISymbol> References { get; } = new List<ISymbol>();

}
