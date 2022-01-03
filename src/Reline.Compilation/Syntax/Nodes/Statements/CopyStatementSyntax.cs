namespace Reline.Compilation.Syntax.Nodes;

public sealed record class CopyStatementSyntax(
	SyntaxToken CopyKeyword,
	IExpressionSyntax Source,
	SyntaxToken ToKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IManipulationStatementSyntax {

	SyntaxToken IManipulationStatementSyntax.SourceKeyword => CopyKeyword;
	SyntaxToken IManipulationStatementSyntax.TargetKeyword => ToKeyword;

}
