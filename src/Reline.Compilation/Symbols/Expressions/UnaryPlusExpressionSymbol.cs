namespace Reline.Compilation.Symbols;

public sealed class UnaryPlusExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public bool IsConstant => Expression.IsConstant;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitUnaryPlus(this);

}
