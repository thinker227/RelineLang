namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryPlusExpressionSyntax(
	SyntaxToken PlusToken,
	IExpressionSyntax Expression
) : SyntaxNode, IUnaryExpressionSyntax {

	SyntaxToken IUnaryExpressionSyntax.UnaryToken => PlusToken;

}
