namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ParameterListSyntax(
	SyntaxToken OpenBracketToken,
	ImmutableArray<SyntaxToken> Parameters,
	SyntaxToken CloseBracketToken
) : SyntaxNode {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitParameterList(this);
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(OpenBracketToken.Span, CloseBracketToken.Span);

}
