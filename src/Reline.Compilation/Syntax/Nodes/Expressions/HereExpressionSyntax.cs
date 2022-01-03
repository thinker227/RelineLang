namespace Reline.Compilation.Syntax.Nodes;

public sealed record class HereExpressionSyntax(
	SyntaxToken HereKeyword
) : SyntaxNode, ITokenExpressionSyntax {

	SyntaxToken ITokenExpressionSyntax.Token => HereKeyword;

}
