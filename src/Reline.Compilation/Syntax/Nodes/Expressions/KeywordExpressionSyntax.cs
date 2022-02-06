namespace Reline.Compilation.Syntax.Nodes;

public sealed record class KeywordExpressionSyntax(
	SyntaxToken Keyword
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitKeywordExpression(this);

}
