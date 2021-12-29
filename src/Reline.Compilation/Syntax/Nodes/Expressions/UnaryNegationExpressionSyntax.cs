namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryNegationExpressionSyntax(
	SyntaxToken MinusToken,
	IExpressionSyntax Expression
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		new(MinusToken.Span.Start, Expression.Span.End);

}
