namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents an expression just containing a <see cref="SyntaxToken"/>.
/// </summary>
public interface ITokenExpressionSyntax : IExpressionSyntax {

	/// <summary>
	/// The token of the expression.
	/// </summary>
	SyntaxToken Token { get; }

}
