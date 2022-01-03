namespace Reline.Compilation.Syntax.Nodes;

public sealed record class EndExpressionSyntax(
	SyntaxToken EndKeyword
) : SyntaxNode, ITokenExpressionSyntax {

	SyntaxToken ITokenExpressionSyntax.Token => EndKeyword;

}
