namespace Reline.Compilation.Syntax.Nodes;

public abstract record class SyntaxNode : IVisitable {

	public void Accept(IVisitor visitor) =>
		visitor.Visit(this);
	public T Accept<T>(IVisitor<T> visitor) =>
		visitor.Visit(this);

}
