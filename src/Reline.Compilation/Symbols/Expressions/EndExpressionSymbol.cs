namespace Reline.Compilation.Symbols;

public sealed class EndExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => false;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitEnd(this);

}
