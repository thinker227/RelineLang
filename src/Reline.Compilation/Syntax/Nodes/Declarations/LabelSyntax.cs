namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LabelSyntax(
	IdentifierSyntax Identifier,
	SyntaxToken ColonToken
) : SyntaxNode, ISyntaxNode {

	public TextSpan Span =>
		new(Identifier.Span.Start, ColonToken.Span.End);

}
