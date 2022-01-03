namespace Reline.Compilation.Syntax.Nodes;

public interface ITokenExpressionSyntax : IExpressionSyntax {

	SyntaxToken Token { get; }

}
