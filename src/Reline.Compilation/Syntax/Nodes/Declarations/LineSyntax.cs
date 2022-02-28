namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LineSyntax(
	int LineNumber,
	LabelSyntax? Label,
	IStatementSyntax? Statement,
	SyntaxToken NewlineToken
) : SyntaxNode {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitLine(this);
	public override IEnumerable<ISyntaxNode> GetChildren() {
		if (Label is not null) yield return Label;
		if (Statement is not null) yield return Statement;
	}

}
