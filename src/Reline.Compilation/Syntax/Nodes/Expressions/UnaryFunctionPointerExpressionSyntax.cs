namespace Reline.Compilation.Syntax.Nodes;

public sealed record class UnaryFunctionPointerExpressionSyntax(
	SyntaxToken StarToken,
	IdentifierSyntax Identifier
) : SyntaxNode, IExpressionSyntax;
