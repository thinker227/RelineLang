using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

public sealed class BinaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Left { get; set; } = null!;
	public IExpressionSymbol Right { get; set; } = null!;
	public BinaryOperatorType OperatorType { get; set; }

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinary(this);

}
