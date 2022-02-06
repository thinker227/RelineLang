using Reline.Compilation.Syntax;

namespace Reline.Compilation.Symbols;

public sealed class BinaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Left { get; set; } = null!;
	public IExpressionSymbol Right { get; set; } = null!;
	public bool IsConstant => Left.IsConstant && Right.IsConstant;
	public BinaryOperatorType OperatorType { get; set; }

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinary(this);

}
