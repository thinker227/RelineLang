namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionPointerExpressionSyntax(
	SyntaxToken StarToken,
	SyntaxToken Identifier
) : SyntaxNode, IExpressionSyntax;
