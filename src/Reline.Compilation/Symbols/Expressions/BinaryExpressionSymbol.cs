using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a binary expression.
/// </summary>
public sealed class BinaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The left operand of the expression.
	/// </summary>
	public IExpressionSymbol Left { get; set; } = null!;
	/// <summary>
	/// The right operand of the expression.
	/// </summary>
	public IExpressionSymbol Right { get; set; } = null!;
	/// <summary>
	/// The operator of the expression.
	/// </summary>
	public BinaryOperatorType OperatorType { get; set; }

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinary(this);

}
