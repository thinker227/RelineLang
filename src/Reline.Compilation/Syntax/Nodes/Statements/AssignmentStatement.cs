namespace Reline.Compilation.Syntax.Nodes;

public sealed record class AssignmentStatementSyntax(
	SyntaxToken Identifier,
	SyntaxToken EqualsToken,
	IExpressionSyntax Initializer
) : SyntaxNode, IStatementSyntax;
