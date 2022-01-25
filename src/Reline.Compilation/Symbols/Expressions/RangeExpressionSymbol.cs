namespace Reline.Compilation.Symbols;

public sealed class RangeExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitRange(this);

}
