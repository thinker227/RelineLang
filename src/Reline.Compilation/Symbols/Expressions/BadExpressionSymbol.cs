namespace Reline.Compilation.Symbols;

public sealed class BadExpressionSymbol : SymbolNode, IExpressionSymbol {

	public bool IsConstant => throw new NotSupportedException();



	public T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotSupportedException();

}
