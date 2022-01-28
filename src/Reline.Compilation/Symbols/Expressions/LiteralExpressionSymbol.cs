namespace Reline.Compilation.Symbols;

public sealed class LiteralExpressionSymbol : SymbolNode, IExpressionSymbol {

	public LiteralValue Literal { get; set; }
	public bool IsConstant => true;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitLiteral(this);

}
