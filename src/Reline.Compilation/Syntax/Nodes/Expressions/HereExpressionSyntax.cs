namespace Reline.Compilation.Syntax.Nodes;

public sealed record class HereExpressionSyntax(
	SyntaxToken HereKeyword
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		HereKeyword.Span;

}
