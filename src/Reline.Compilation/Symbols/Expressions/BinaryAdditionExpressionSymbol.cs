namespace Reline.Compilation.Symbols;

public sealed class BinaryAdditionExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitBinaryAddition(this);

}
