namespace Reline.Compilation.Symbols.Statements;

public sealed class ReturnStatementSymbol : SymbolNode, IStatementSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;

}
