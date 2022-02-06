﻿namespace Reline.Compilation.Syntax.Nodes;

public sealed record class CopyStatementSyntax(
	SyntaxToken CopyKeyword,
	IExpressionSyntax Source,
	SyntaxToken ToKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IManipulationStatementSyntax {

	SyntaxToken IManipulationStatementSyntax.SourceKeyword => CopyKeyword;
	SyntaxToken IManipulationStatementSyntax.TargetKeyword => ToKeyword;

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitCopyStatement(this);

}
