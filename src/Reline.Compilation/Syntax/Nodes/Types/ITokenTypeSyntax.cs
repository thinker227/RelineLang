namespace Reline.Compilation.Syntax.Nodes;

public interface ITokenTypeSyntax : ITypeSyntax {

	SyntaxToken Token { get; }

}
