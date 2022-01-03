namespace Reline.Compilation.Syntax.Nodes;

public sealed record class AssignmentStatementSyntax(
	IdentifierSyntax Identifier,
	SyntaxToken EqualsToken,
	IExpressionSyntax Initializer
) : SyntaxNode, IStatementSyntax;
