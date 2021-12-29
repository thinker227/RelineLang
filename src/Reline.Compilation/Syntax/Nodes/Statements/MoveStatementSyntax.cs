namespace Reline.Compilation.Syntax.Nodes;

public sealed record class MoveStatementSyntax(
	SyntaxToken MoveKeyword,
	IExpressionSyntax Source,
	SyntaxToken ToKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IStatementSyntax {

	public TextSpan Span =>
		new(MoveKeyword.Span.Start, Target.Span.End);

}
