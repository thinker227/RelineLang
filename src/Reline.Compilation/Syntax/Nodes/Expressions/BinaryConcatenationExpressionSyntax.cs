namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryConcatenationExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken LessThanToken,
	IExpressionSyntax Right
) : IBinaryExpressionSyntax;
