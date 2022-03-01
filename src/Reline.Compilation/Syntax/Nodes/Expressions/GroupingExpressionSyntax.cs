namespace Reline.Compilation.Syntax.Nodes;

public sealed record class GroupingExpressionSyntax(
	SyntaxToken OpenBracketToken,
	IExpressionSyntax Expression,
	SyntaxToken CloseBracketToken
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitGroupingExpression(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Expression;
	}
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(OpenBracketToken.Span, CloseBracketToken.Span);

}
