namespace Reline.Compilation.Symbols;

public sealed class BinaryMultiplicationExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitBinaryMultiplication(this);

}
