namespace Reline.Compilation.Syntax.Nodes;

public interface IUnaryExpressionSyntax : IExpressionSyntax {

	SyntaxToken UnaryToken { get; }
	IExpressionSyntax Expression { get; }

}
