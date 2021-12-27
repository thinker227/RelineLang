namespace Reline.Compilation.Syntax.Nodes;

public sealed record class EndExpressionSyntax(
	SyntaxToken EndKeyword
) : SyntaxNode, IExpressionSyntax;
