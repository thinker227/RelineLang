namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken OperatorToken,
	IExpressionSyntax Right
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitBinaryExpression(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Left;
		yield return Right;
	}
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(Left.GetTextSpan(), Right.GetTextSpan());

}
