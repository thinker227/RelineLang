namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryAdditionExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken PlusToken,
	IExpressionSyntax Right
) : IBinaryExpressionSyntax;
