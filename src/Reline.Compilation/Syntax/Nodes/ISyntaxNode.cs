namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a syntax node.
/// </summary>
public interface ISyntaxNode {

	/// <summary>
	/// The span of the syntax node in the source text.
	/// </summary>
	TextSpan Span { get; }

}
