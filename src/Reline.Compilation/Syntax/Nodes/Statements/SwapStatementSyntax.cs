﻿namespace Reline.Compilation.Syntax.Nodes;

public sealed record class SwapStatementSyntax(
	SyntaxToken SwapKeyword,
	IExpressionSyntax Source,
	SyntaxToken WithKeyword,
	IExpressionSyntax Target
) : SyntaxNode, IStatementSyntax {

	public TextSpan Span =>
		new(SwapKeyword.Span.Start, Target.Span.End);

}
