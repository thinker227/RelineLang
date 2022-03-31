namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an identifier expression.
/// </summary>
public sealed class IdentifierExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The identifier of the expression.
	/// </summary>
	public IIdentifiableSymbol Identifier { get; set; } = null!;



	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitVariable(this);

	public override IEnumerable<ISymbol> GetChildren() {
		yield return Identifier;
	}

}
