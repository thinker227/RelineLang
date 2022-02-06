namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LabelSyntax(
	SyntaxToken Identifier,
	SyntaxToken ColonToken
) : SyntaxNode {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitLabel(this);

}
