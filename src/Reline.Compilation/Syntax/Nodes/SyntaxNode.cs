using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents an abstract syntax node.
/// </summary>
public abstract record class SyntaxNode : ISyntaxNode {

	public void Accept(IVisitor<ISyntaxNode> visitor) =>
		visitor.Visit(this);
	public TResult Accept<TResult>(IVisitor<ISyntaxNode, TResult> visitor) =>
		visitor.Visit(this);

}
