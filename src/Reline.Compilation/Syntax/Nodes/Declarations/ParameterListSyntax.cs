namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ParameterListSyntax(
	SyntaxToken OpenBracketToken,
	ImmutableArray<TypedIdentifierSyntax> Parameters,
	SyntaxToken CloseBracketToken
) : SyntaxNode;
