using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a binary expression.
/// </summary>
public sealed class BinaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The left operand of the expression.
	/// </summary>
	public IExpressionSymbol Left { get; }
	/// <summary>
	/// The right operand of the expression.
	/// </summary>
	public IExpressionSymbol Right { get; }
	/// <summary>
	/// The operator of the expression.
	/// </summary>
	public BinaryOperatorType OperatorType { get; }



	internal BinaryExpressionSymbol(IExpressionSymbol left, BinaryOperatorType operatorType, IExpressionSymbol right) {
		Left = left;
		OperatorType = operatorType;
		Right = right;
	}



	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinary(this);

	public override IEnumerable<ISymbol> GetChildren() {
		yield return Left;
		yield return Right;
	}

}
