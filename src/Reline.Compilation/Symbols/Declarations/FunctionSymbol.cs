namespace Reline.Compilation.Symbols;

public sealed class FunctionSymbol : SymbolNode, IIdentifiableSymbol {

	public string Identifier { get; set; } = null!;
	public IExpressionSymbol BodyExpression { get; set; } = null!;
	public ICollection<LineSymbol> Body { get; } = new List<LineSymbol>();
	public ICollection<ParameterSymbol> Parameters { get; } = new List<ParameterSymbol>();

}
