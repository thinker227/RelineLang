namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a binary expression.
/// </summary>
public interface IBinaryExpressionSyntax : IExpressionSyntax {

	/// <summary>
	/// The left-hand side of the expression.
	/// </summary>
	IExpressionSyntax Left { get; }
	/// <summary>
	/// The right-hand side of the expression.
	/// </summary>
	IExpressionSyntax Right { get; }

}
