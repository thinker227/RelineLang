namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a unary expression.
/// </summary>
public interface IUnaryExpressionSyntax : IExpressionSyntax {

	/// <summary>
	/// The token containing the unary operation.
	/// </summary>
	SyntaxToken UnaryToken { get; }
	/// <summary>
	/// The expression the unary operator is applied to.
	/// </summary>
	IExpressionSyntax Expression { get; }

}
