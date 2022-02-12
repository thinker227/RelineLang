using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

public sealed class UnaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Expression { get; set; } = null!;
	public UnaryOperatorType OperatorType { get; set; }

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitUnary(this);

}
