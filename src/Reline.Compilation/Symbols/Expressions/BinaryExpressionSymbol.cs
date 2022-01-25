namespace Reline.Compilation.Symbols;

public abstract class BinaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Left { get; set; } = null!;
	public IExpressionSymbol Right { get; set; } = null!;
	public virtual bool IsConstant => Left.IsConstant && Right.IsConstant;

	public abstract T Accept<T>(IExpressionVisitor<T> visitor);

}
