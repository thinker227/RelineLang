namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionInvocationExpressionSyntax(
	SyntaxToken Identifier,
	SyntaxToken OpenBracketToken,
	ImmutableArray<IExpressionSyntax> Arguments,
	SyntaxToken CloseBracketToken
) : SyntaxNode, IExpressionSyntax;
