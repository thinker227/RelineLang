namespace Reline.Compilation.Syntax.Nodes;

public record class ExpressionStatementSyntax(
	IExpressionSyntax Expression
) : SyntaxNode, IStatementSyntax {

	public TextSpan Span =>
		Expression.Span;

}
