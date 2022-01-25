namespace Reline.Compilation.Symbols;

public sealed class VariableExpressionSymbol : SymbolNode, IExpressionSymbol {

	public VariableSymbol Variable { get; set; } = null!;
	public bool IsConstant => false;

	public T Accept<T>(IExpressionVisitor<T> visitor) =>
		visitor.VisitVariable(this);

}
