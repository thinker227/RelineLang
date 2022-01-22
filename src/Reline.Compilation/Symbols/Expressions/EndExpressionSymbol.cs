namespace Reline.Compilation.Symbols;

public sealed class EndExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => false;

}
