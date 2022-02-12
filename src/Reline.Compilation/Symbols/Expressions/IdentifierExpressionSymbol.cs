namespace Reline.Compilation.Symbols;

public sealed class IdentifierExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IIdentifiableSymbol Identifier { get; set; } = null!;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitVariable(this);

}
