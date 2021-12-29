namespace Reline.Compilation.Syntax.Nodes;

public sealed record class AssignmentStatementSyntax(
	IdentifierSyntax Identifier,
	SyntaxToken EqualsToken,
	IExpressionSyntax Initializer
) : SyntaxNode, IStatementSyntax {

	public TextSpan Span =>
		new(Identifier.Span.Start, Initializer.Span.End);

}
