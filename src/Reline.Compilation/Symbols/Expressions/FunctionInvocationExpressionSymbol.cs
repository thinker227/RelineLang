namespace Reline.Compilation.Symbols;

public sealed class FunctionInvocationExpressionSymbol : SymbolNode, IExpressionSymbol {

	public FunctionSymbol Function { get; set; } = null!;
	public ICollection<IExpressionSymbol> Arguments { get; } = new List<IExpressionSymbol>();
	public bool IsConstant => false;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitFunctionInvocation(this);

}
