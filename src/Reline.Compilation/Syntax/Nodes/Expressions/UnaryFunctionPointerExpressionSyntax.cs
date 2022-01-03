namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryFunctionPointerExpressionSyntax(
	SyntaxToken StarToken,
	IdentifierExpressionSyntax Identifier
) : SyntaxNode, IUnaryExpressionSyntax {

	SyntaxToken IUnaryExpressionSyntax.UnaryToken => StarToken;
	IExpressionSyntax IUnaryExpressionSyntax.Expression => Identifier;

}
