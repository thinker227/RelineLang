namespace Reline.Compilation.Symbols;

public sealed class EndExpressionSymbol : SymbolNode, IExpressionSymbol {

	public ITypeSymbol Type => NumberType.Instance;
	public bool IsConstant => false;

}
