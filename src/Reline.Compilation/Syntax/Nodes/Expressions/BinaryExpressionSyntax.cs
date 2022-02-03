namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken OperatorToken,
	IExpressionSyntax Right,
	BinaryOperatorType OperatorType
) : SyntaxNode, IExpressionSyntax;
