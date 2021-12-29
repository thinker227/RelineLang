namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryFunctionPointerExpressionSyntax(
	SyntaxToken StarToken,
	IdentifierSyntax Identifier
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		new(StarToken.Span.Start, Identifier.Span.End);

}
