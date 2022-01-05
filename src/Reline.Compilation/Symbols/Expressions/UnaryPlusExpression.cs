namespace Reline.Compilation.Symbols;

public sealed class UnaryPlusExpression : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public ITypeSymbol Type => Expression.Type;
	public bool IsConstant => Expression.IsConstant;

}
