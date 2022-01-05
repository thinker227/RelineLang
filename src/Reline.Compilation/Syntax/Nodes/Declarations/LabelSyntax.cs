namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LabelSyntax(
	SyntaxToken Identifier,
	SyntaxToken ColonToken
) : SyntaxNode;
