namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionInvocationExpressionSyntax(
	SyntaxToken Identifier,
	SyntaxToken OpenBracketToken,
	ImmutableArray<IExpressionSyntax> Arguments,
	SyntaxToken CloseBracketToken
) : SyntaxNode, IExpressionSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitFunctionInvocationExpression(this);
	public override IEnumerable<ISyntaxNode> GetChildren() =>
		Arguments;

}
