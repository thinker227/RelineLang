namespace Reline.Compilation.Syntax.Nodes;

public sealed record class CopyStatementSyntax(
	SyntaxToken CopyKeyword,
	IExpressionSyntax Source,
	SyntaxToken ToKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IStatementSyntax {

	public TextSpan Span =>
		new(CopyKeyword.Span.Start, Target.Span.End);

}
