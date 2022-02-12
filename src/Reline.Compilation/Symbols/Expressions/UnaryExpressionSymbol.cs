using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a unary expression.
/// </summary>
public sealed class UnaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The operand of the expression.
	/// </summary>
	public IExpressionSymbol Expression { get; set; } = null!;
	/// <summary>
	/// The operator of the expression.
	/// </summary>
	public UnaryOperatorType OperatorType { get; set; }

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitUnary(this);

}
