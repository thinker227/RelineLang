namespace Reline.Compilation.Syntax.Nodes;

public sealed record class IdentifierExpressionSyntax(
	IdentifierSyntax Identifier
) : SyntaxNode, IExpressionSyntax;
