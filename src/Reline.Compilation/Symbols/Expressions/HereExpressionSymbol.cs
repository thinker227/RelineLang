namespace Reline.Compilation.Symbols;

public sealed class HereExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => false;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitHere(this);

}
