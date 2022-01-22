namespace Reline.Compilation.Symbols;

public sealed class HereExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => false;

}
