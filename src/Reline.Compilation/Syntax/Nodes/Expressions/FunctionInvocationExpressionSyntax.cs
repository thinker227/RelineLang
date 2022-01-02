namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionInvocationExpressionSyntax(
	IdentifierSyntax Identifier,
	SyntaxToken OpenBracketToken,
	ImmutableArray<IExpressionSyntax> Arguments,
	SyntaxToken CloseBracketToken
) : SyntaxNode, IExpressionSyntax {

	public TextSpan Span =>
		new(Identifier.Span.Start,
			Arguments.Length > 0 ? Arguments[^1].Span.End : Identifier.Span.End);

}
