namespace Reline.Compilation.Symbols;

public sealed class HereExpressionSymbol : SymbolNode, IExpressionSymbol {

	public ITypeSymbol Type => NumberType.Instance;
	public bool IsConstant => false;

}
