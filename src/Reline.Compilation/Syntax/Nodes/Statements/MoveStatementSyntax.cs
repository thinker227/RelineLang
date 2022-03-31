namespace Reline.Compilation.Syntax.Nodes;

public sealed record class MoveStatementSyntax(
	SyntaxToken MoveKeyword,
	IExpressionSyntax Source,
	SyntaxToken ToKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IManipulationStatementSyntax {

	SyntaxToken IManipulationStatementSyntax.SourceKeyword => MoveKeyword;
	SyntaxToken IManipulationStatementSyntax.TargetKeyword => ToKeyword;

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitMoveStatement(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Source;
		yield return Target;
	}
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(MoveKeyword.Span, Target.GetTextSpan());

}
