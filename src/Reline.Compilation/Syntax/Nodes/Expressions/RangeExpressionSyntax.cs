namespace Reline.Compilation.Syntax.Nodes;

public sealed record class RangeExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken DotDotToken,
	IExpressionSyntax Right
) : IBinaryExpressionSyntax;
