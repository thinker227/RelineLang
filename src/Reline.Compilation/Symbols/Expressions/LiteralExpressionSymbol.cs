namespace Reline.Compilation.Symbols;

public sealed class LiteralExpressionSymbol : SymbolNode, IExpressionSymbol {

	public ILiteralSymbol Literal { get; set; } = null!;
	public ITypeSymbol Type => Literal.Type;
	public bool IsConstant => true;

}
