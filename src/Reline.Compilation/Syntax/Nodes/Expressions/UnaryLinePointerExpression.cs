namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryLinePointerExpressionSyntax(
	SyntaxToken StarToken,
	IExpressionSyntax Expression
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		new(StarToken.Span.Start, Expression.Span.End);

}
