namespace Reline.Compilation.Syntax.Nodes;

public sealed record class BinaryDivisionExpressionSyntax(
	IExpressionSyntax Left,
	SyntaxToken SlashToken,
	IExpressionSyntax Right
) : IBinaryExpressionSyntax;
