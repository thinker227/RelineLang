namespace Reline.Compilation.Syntax.Nodes;

public sealed record class IdentifierExpressionSyntax(
	SyntaxToken Identifier
) : SyntaxNode, IExpressionSyntax;
