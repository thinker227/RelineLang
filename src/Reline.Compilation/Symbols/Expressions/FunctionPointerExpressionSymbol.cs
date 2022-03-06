namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function pointer expression.
/// </summary>
public sealed class FunctionPointerExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The function being pointed to.
	/// </summary>
	public FunctionSymbol Function { get; set; } = null!;



	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitFunctionPointer(this);

	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

}
