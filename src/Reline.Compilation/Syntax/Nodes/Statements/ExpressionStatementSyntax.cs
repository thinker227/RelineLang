namespace Reline.Compilation.Syntax.Nodes;

public record class ExpressionStatementSyntax(
	IExpressionSyntax Expression
) : SyntaxNode, IStatementSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitExpressionStatement(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Expression;
	}
	public override TextSpan GetTextSpan() => Expression.GetTextSpan();

}
