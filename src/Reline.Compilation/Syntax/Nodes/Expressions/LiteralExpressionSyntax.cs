namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LiteralExpressionSyntax(
	SyntaxToken Literal
) : SyntaxNode, IExpressionSyntax;