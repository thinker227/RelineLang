using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents an abstract syntax node.
/// </summary>
public abstract record class SyntaxNode : ISyntaxNode {

	public abstract T Accept<T>(ISyntaxVisitor<T> visitor);

}
