namespace Reline.Compilation.Symbols;

public sealed class UnaryLinePointerExpression : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public bool IsConstant => Expression.IsConstant;

}
