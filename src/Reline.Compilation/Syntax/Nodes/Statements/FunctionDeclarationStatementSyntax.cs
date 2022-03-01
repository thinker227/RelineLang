namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionDeclarationStatementSyntax(
	SyntaxToken FunctionKeyword,
	SyntaxToken Identifier,
	IExpressionSyntax Body,
	ParameterListSyntax? ParameterList
) : SyntaxNode, IStatementSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitFunctionDeclarationStatement(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		yield return Body;
		if (ParameterList is not null) yield return ParameterList;
	}
	public override TextSpan GetTextSpan() => new(
		FunctionKeyword.Span.Start,
		ParameterList?.GetTextSpan().End ??
		Body.GetTextSpan().End);

}
