namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryLinePointerExpressionSyntax(
	SyntaxToken StarToken,
	SyntaxToken OpenSquareToken,
	IExpressionSyntax Expression,
	SyntaxToken CloseSquareToken
) : SyntaxNode, IExpressionSyntax;
// This looks like a unary expression but isn't due to the addition tokens.
