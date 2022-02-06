namespace Reline.Compilation.Symbols;

public sealed class FunctionPointerExpressionSymbol : SymbolNode, IExpressionSymbol {

	public FunctionSymbol Function { get; set; } = null!;
	public bool IsConstant => true;

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitFunctionPointer(this);

}
