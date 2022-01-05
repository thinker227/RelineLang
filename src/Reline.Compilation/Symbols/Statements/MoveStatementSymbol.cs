namespace Reline.Compilation.Symbols;

public sealed class MoveStatementSymbol : SymbolNode, IStatementSymbol {

	public IExpressionSymbol Source { get; set; } = null!;
	public IExpressionSymbol Target { get; set; } = null!;

}
