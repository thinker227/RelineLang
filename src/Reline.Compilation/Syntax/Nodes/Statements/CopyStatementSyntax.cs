namespace Reline.Compilation.Syntax.Nodes;

public sealed record class CopyStatementSyntax(
	SyntaxToken CopyKeyword,
	IExpressionSyntax Source,
	SyntaxToken ToKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IManipulationStatementSyntax {

	SyntaxToken IManipulationStatementSyntax.SourceKeyword => CopyKeyword;
	SyntaxToken IManipulationStatementSyntax.TargetKeyword => ToKeyword;

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitCopyStatement(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Source;
		yield return Target;
	}
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(CopyKeyword.Span, Target.GetTextSpan());

}
