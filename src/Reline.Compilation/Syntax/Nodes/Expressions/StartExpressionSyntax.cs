namespace Reline.Compilation.Syntax.Nodes;

public sealed record class StartExpressionSyntax(
	SyntaxToken StartKeyword
) : SyntaxNode, ITokenExpressionSyntax {

	SyntaxToken ITokenExpressionSyntax.Token => StartKeyword;

}
