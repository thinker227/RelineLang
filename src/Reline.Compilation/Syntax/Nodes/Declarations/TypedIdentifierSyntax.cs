namespace Reline.Compilation.Syntax.Nodes;

public sealed record class TypedIdentifierSyntax(
	ITypeSyntax Type,
	SyntaxToken Identifier
) : SyntaxNode;
