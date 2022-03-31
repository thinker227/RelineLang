namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LiteralExpressionSyntax(
	SyntaxToken Literal
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitLiteralExpression(this);
	public override TextSpan GetTextSpan() => Literal.Span;

}
