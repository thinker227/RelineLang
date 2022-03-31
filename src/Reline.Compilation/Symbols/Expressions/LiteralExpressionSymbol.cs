namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a literal expression.
/// </summary>
public sealed class LiteralExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The literal value of the expression.
	/// </summary>
	public BoundValue Literal { get; set; }



	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitLiteral(this);

	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

}
