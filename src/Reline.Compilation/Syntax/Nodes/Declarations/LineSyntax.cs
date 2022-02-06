﻿namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LineSyntax(
	LabelSyntax? Label,
	IStatementSyntax? Statement,
	SyntaxToken NewlineToken
) : SyntaxNode {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitLine(this);

}
