namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ProgramSyntax(
	ImmutableArray<LineSyntax> Lines
) : SyntaxNode {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitProgram(this);
	public override IEnumerable<ISyntaxNode> GetChildren() =>
		Lines;

}
