namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ReturnStatementSyntax(
	SyntaxToken ReturnKeyword,
	IExpressionSyntax Expression
) : SyntaxNode, IStatementSyntax {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitReturnStatement(this);

}
