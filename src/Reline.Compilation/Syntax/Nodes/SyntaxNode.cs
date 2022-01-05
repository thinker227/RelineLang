using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents an abstract syntax node.
/// </summary>
public abstract record class SyntaxNode : ISyntaxNode {

	public void Accept(IVisitor visitor) =>
		visitor.Visit(this);
	public T Accept<T>(IVisitor<T> visitor) =>
		visitor.Visit(this);

}
