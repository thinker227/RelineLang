namespace Reline.Compilation.Syntax.Nodes;

public sealed record class IdentifierExpressionSyntax(
	SyntaxToken Identifier
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitIdentifierExpression(this);
	public override TextSpan GetTextSpan() => Identifier.Span;

}
