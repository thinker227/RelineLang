namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionPointerExpressionSyntax(
	SyntaxToken StarToken,
	SyntaxToken Identifier
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitFunctionPointerExpression(this);
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(StarToken.Span, Identifier.Span);

}
