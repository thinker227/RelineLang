namespace Reline.Compilation.Symbols;

public sealed class UnaryPlusExpression : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public bool IsConstant => Expression.IsConstant;

}
