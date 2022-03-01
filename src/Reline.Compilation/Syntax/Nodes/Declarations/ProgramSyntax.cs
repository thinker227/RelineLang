namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ProgramSyntax(
	ImmutableArray<LineSyntax> Lines
) : SyntaxNode {

	public override T Accept<T>(ISyntaxVisitor<T> visitor) => visitor.VisitProgram(this);
	public override IEnumerable<ISyntaxNode> GetChildren() =>
		Lines;
	public override TextSpan GetTextSpan() =>
		TextSpan.FromBounds(Lines[0].GetTextSpan(), Lines[^1].GetTextSpan());

}
