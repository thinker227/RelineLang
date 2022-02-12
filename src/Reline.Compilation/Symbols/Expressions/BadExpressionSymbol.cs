namespace Reline.Compilation.Symbols;

public sealed class BadExpressionSymbol : SymbolNode, IExpressionSymbol {

	public T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotSupportedException();

}
