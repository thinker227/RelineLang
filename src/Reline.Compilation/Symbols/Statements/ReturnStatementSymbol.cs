namespace Reline.Compilation.Symbols;

public sealed class ReturnStatementSymbol : SymbolNode, IStatementSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;

}
