namespace Reline.Compilation.Syntax.Nodes.Types;

public sealed record class StringTypeSyntax(
	SyntaxToken StringKeyword
) : SyntaxNode, ITokenTypeSyntax {

	SyntaxToken ITokenTypeSyntax.Token => StringKeyword;

}
