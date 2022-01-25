namespace Reline.Compilation.Symbols;

public sealed class BinarySubtractionExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitBinarySubtraction(this);

}
