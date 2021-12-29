namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryPlusExpressionSyntax(
	SyntaxToken PlusToken,
	IExpressionSyntax Expression
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		new(PlusToken.Span.Start, Expression.Span.End);

}
