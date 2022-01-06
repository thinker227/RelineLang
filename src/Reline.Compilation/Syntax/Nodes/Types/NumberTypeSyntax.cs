namespace Reline.Compilation.Syntax.Nodes;

public sealed record class NumberTypeSyntax(
	SyntaxToken NumberKeyword
) : SyntaxNode, ITokenTypeSyntax {

	SyntaxToken ITokenTypeSyntax.Token => NumberKeyword;

}
