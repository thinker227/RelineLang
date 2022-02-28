namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a syntax node.
/// </summary>
public interface ISyntaxNode : INode<ISyntaxNode> {

	public T Accept<T>(ISyntaxVisitor<T> visitor);

}
