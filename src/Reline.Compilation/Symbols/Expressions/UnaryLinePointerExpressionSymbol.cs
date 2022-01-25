namespace Reline.Compilation.Symbols;

public sealed class UnaryLinePointerExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public bool IsConstant => Expression.IsConstant;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitUnaryLinePointer(this);

}
