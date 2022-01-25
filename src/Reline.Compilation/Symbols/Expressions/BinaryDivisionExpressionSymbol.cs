namespace Reline.Compilation.Symbols;

public sealed class BinaryDivisionExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitBinaryDivision(this);

}
