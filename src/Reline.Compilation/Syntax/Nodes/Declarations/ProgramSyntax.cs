namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ProgramSyntax(
	ImmutableArray<LineSyntax> Lines
) : SyntaxNode, ISyntaxNode {

	public TextSpan Span =>
		new(Lines[0].Span.Start, Lines[^1].Span.End);

}
