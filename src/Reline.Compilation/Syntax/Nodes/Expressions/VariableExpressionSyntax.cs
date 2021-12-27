namespace Reline.Compilation.Syntax.Nodes;

public sealed record class VariableExpressionSyntax(
	IdentifierSyntax Identifier
) : SyntaxNode, IExpressionSyntax;
