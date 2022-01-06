namespace Reline.Compilation.Syntax.Nodes.Types;

public sealed record class NumberTypeSyntax(
	SyntaxToken NumberKeyword
) : SyntaxNode, ITokenTypeSyntax {

	SyntaxToken ITokenTypeSyntax.Token => NumberKeyword;

}
