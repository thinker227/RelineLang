namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LineSyntax(
	LabelSyntax? Label,
	IStatementSyntax? Statement,
	SyntaxToken NewlineToken
) : SyntaxNode, ISyntaxNode {

	public TextSpan Span =>
		new(Label?.Span.Start ?? Statement?.Span.Start ?? NewlineToken.Span.Start, NewlineToken.Span.End);

}
