namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LinePointerExpressionSyntax(
	SyntaxToken StarToken,
	SyntaxToken Identifier
) : SyntaxNode, IExpressionSyntax;
