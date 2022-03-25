namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function invocation expression.
/// </summary>
public sealed class FunctionInvocationExpressionSymbol : SymbolNode, IExpressionSymbol {
	
	/// <summary>
	/// The function being invoked.
	/// </summary>
	public IFunctionSymbol Function { get; }
	/// <summary>
	/// The arguments passed to the function invocation.
	/// </summary>
	public IReadOnlyCollection<IExpressionSymbol> Arguments { get; }



	internal FunctionInvocationExpressionSymbol(IFunctionSymbol function, IReadOnlyCollection<IExpressionSymbol> arguments) {
		Function = function;
		Arguments = arguments;
	}



	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitFunctionInvocation(this);

	public override IEnumerable<ISymbol> GetChildren() =>
		Arguments;

}
