namespace Reline.Compilation.Symbols;

public sealed class LiteralExpressionSymbol : SymbolNode, IExpressionSymbol {

	public ILiteralSymbol Literal { get; set; } = null!;
	public bool IsConstant => true;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitLiteral(this);

}
