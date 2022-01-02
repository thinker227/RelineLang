namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryLinePointerExpressionSyntax(
	SyntaxToken StarToken,
	SyntaxToken OpenSquareToken,
	IExpressionSyntax Expression,
	SyntaxToken CloseSquareToken
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		new(StarToken.Span.Start, Expression.Span.End);

}
