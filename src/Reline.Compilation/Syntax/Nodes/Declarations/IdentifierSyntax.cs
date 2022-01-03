namespace Reline.Compilation.Syntax.Nodes;

public sealed record class IdentifierSyntax(
	SyntaxToken Name
) : SyntaxNode, ISyntaxNode {

	public TextSpan Span =>
		Name.Span;

}
