namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a literal expression.
/// </summary>
public sealed class LiteralExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The literal value of the expression.
	/// </summary>
	public LiteralValue Literal { get; set; }

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitLiteral(this);

}
