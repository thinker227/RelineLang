namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an identifier expression.
/// </summary>
public sealed class IdentifierExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The identifier of the expression.
	/// </summary>
	public IIdentifiableSymbol Identifier { get; }



	internal IdentifierExpressionSymbol(IIdentifiableSymbol identifier) {
		Identifier = identifier;
	}



	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitVariable(this);

	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

}
