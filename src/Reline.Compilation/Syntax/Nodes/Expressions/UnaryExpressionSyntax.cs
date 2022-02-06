namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryExpressionSyntax(
	SyntaxToken OperatorToken,
	IExpressionSyntax Expression
) : SyntaxNode, IExpressionSyntax;
