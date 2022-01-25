namespace Reline.Compilation.Symbols;

public sealed class BinaryModuloExpressionSymbol : BinaryExpressionSymbol {

	public override T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitBinaryModulo(this);

}
