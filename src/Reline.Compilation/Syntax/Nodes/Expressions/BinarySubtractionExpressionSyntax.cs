namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinarySubtractionExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken MinusToken,
	IExpressionSyntax Right
) : SyntaxNode, IBinaryExpressionSyntax;
