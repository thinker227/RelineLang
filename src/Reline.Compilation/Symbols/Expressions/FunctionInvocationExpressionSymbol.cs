namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function invocation expression.
/// </summary>
public sealed class FunctionInvocationExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The function being invoked.
	/// </summary>
	public IFunctionSymbol Function { get; set; } = null!;
	/// <summary>
	/// The arguments passed to the function invocation.
	/// </summary>
	public ICollection<IExpressionSymbol> Arguments { get; } = new List<IExpressionSymbol>();



	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitFunctionInvocation(this);

	public override IEnumerable<ISymbol> GetChildren() =>
		Arguments;

}
