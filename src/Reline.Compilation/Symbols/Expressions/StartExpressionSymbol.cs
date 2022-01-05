namespace Reline.Compilation.Symbols;

public sealed class StartExpressionSymbol : SymbolNode, IExpressionSymbol {

	public ITypeSymbol Type => NumberType.Instance;
	public bool IsConstant => false;

}
