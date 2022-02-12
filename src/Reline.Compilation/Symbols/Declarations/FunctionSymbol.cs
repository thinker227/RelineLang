namespace Reline.Compilation.Symbols;

public sealed class FunctionSymbol : SymbolNode, IIdentifiableSymbol {

	public string Identifier { get; set; } = null!;
	public FunctionDeclarationStatementSymbol Declaration { get; set; } = null!;
	public IExpressionSymbol RangeExpression { get; set; } = null!;
	public RangeLiteral Range { get; set; }
	public ICollection<LineSymbol> Body { get; } = new List<LineSymbol>();
	public ICollection<ParameterSymbol> Parameters { get; } = new List<ParameterSymbol>();
	public IList<ISymbol> References { get; } = new List<ISymbol>();

}
