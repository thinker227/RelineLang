namespace Reline.Compilation.Syntax.Nodes;

public sealed record class GroupingExpressionSyntax(
	SyntaxToken OpenBracketToken,
	IExpressionSyntax Expression,
	SyntaxToken CloseBracketToken
) : SyntaxNode, IExpressionSyntax;
}
