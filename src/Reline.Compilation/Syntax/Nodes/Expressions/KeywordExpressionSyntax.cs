namespace Reline.Compilation.Syntax.Nodes;

public sealed record class KeywordExpressionSyntax(
	SyntaxToken Keyword,
	KeywordExpressionType KeywordType
) : SyntaxNode, IExpressionSyntax;
