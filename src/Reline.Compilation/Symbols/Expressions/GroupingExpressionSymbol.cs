namespace Reline.Compilation.Symbols;

public sealed class GroupingExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public bool IsConstant => Expression.IsConstant;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitGrouping(this);

}
