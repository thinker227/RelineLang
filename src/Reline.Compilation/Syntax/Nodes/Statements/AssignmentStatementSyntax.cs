namespace Reline.Compilation.Syntax.Nodes;

public sealed record class AssignmentStatementSyntax(
	SyntaxToken Identifier,
	SyntaxToken EqualsToken,
	IExpressionSyntax Initializer
) : SyntaxNode, IStatementSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitAssignmentStatement(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Initializer;
	}
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(Identifier.Span, Initializer.GetTextSpan());

}
