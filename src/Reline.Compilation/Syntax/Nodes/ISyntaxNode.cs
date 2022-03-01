namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a syntax node.
/// </summary>
public interface ISyntaxNode : INode<ISyntaxNode> {

	/// <summary>
	/// Accepts a <see cref="ISyntaxVisitor{T}"/>.
	/// </summary>
	/// <typeparam name="T">The return type of the visitor.</typeparam>
	/// <param name="visitor">The <see cref="ISyntaxVisitor{T}"/> to accept.</param>
	/// <returns>The return value of <paramref name="visitor"/>.</returns>
	T Accept<T>(ISyntaxVisitor<T> visitor);
	/// <summary>
	/// The the span of the syntax node in the source text.
	/// </summary>
	/// <returns>A <see cref="TextSpan"/> representing the span
	/// of the syntax node in the source text.</returns>
	TextSpan GetTextSpan();

}
