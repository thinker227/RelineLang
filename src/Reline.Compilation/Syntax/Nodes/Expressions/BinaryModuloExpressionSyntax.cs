namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryModuloExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken PercentToken,
	IExpressionSyntax Right
) : IBinaryExpressionSyntax;
