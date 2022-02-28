namespace Reline.Compilation.Syntax.Nodes;

public sealed record class SwapStatementSyntax(
	SyntaxToken SwapKeyword,
	IExpressionSyntax Source,
	SyntaxToken WithKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IManipulationStatementSyntax {

	SyntaxToken IManipulationStatementSyntax.SourceKeyword => SwapKeyword;
	SyntaxToken IManipulationStatementSyntax.TargetKeyword => WithKeyword;

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitSwapStatement(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Source;
		yield return Target;
	}

}
