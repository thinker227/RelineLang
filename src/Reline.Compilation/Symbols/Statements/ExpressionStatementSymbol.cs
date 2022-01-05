namespace Reline.Compilation.Symbols;

public sealed class ExpressionStatementSymbol : SymbolNode, IStatementSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;

}
