namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryExpressionSyntax(
	SyntaxToken OperatorToken,
	IExpressionSyntax Expression
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitUnaryExpression(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Expression;
	}

}
