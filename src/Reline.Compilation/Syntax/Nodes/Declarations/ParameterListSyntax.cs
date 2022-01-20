namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ParameterListSyntax(
	SyntaxToken OpenBracketToken,
	ImmutableArray<SyntaxToken> Parameters,
	SyntaxToken CloseBracketToken
) : SyntaxNode;
