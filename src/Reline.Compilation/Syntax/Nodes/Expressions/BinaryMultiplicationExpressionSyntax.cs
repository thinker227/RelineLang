namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryMultiplicationExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken StarToken,
	IExpressionSyntax Right
) : SyntaxNode, IBinaryExpressionSyntax;
