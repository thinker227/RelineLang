namespace Reline.Compilation.Syntax.Nodes;

public interface IBinaryExpressionSyntax : IExpressionSyntax {

	IExpressionSyntax Left { get; }
	IExpressionSyntax Right { get; }

}
