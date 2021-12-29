namespace Reline.Compilation.Syntax.Nodes;

public abstract record class BinaryExpressionSyntax : IExpressionSyntax {

	public abstract IExpressionSyntax Left { get; init; }
	public abstract IExpressionSyntax Right { get; init; }
	public TextSpan Span =>
		new(Left.Span.Start, Right.Span.End);

}
