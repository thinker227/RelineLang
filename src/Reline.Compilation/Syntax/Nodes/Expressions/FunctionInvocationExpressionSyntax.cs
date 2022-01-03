namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionInvocationExpressionSyntax(
	IdentifierSyntax Identifier,
	SyntaxToken OpenBracketToken,
	ImmutableArray<IExpressionSyntax> Arguments,
	SyntaxToken CloseBracketToken
) : SyntaxNode, IExpressionSyntax;
