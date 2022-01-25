namespace Reline.Compilation.Symbols;

public sealed class BinaryConcatenationExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitBinaryConcatenation(this);

}
