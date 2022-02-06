namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionDeclarationStatementSyntax(
	SyntaxToken FunctionKeyword,
	SyntaxToken Identifier,
	IExpressionSyntax Body,
	ParameterListSyntax? ParameterList
) : SyntaxNode, IStatementSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitFunctionDeclarationStatement(this);

}
