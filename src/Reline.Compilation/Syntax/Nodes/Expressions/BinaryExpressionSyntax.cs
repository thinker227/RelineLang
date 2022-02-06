namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken OperatorToken,
	IExpressionSyntax Right
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitBinaryExpression(this);

}
