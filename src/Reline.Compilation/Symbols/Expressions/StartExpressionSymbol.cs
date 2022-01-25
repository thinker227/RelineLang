namespace Reline.Compilation.Symbols;

public sealed class StartExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => false;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitStart(this);

}
