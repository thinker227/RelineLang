namespace Reline.Compilation.Symbols;

public sealed class UnaryLinePointerExpression : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public ITypeSymbol Type => Expression.Type;
	public bool IsConstant => Expression.IsConstant;

}
