namespace Reline.Compilation.Symbols;

public sealed class StartExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => false;

}
