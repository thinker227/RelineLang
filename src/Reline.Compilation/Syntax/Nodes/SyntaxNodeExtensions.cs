using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax.Nodes;

public static class SyntaxNodeExtensions {

	/// <summary>
	/// Adds a diagnostic to a <see cref="SyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TNode">The type of the syntax node.</typeparam>
	/// <param name="node">The source node.</param>
	/// <param name="diagnostic">The diagnostic to add.</param>
	/// <returns>A new syntax node of type <typeparamref name="TNode"/>
	/// with <paramref name="diagnostic"/> added to its diagnostics.</returns>
	public static TNode AddDiagnostic<TNode>(this TNode node, Diagnostic diagnostic) where TNode : SyntaxNode {
		var newDiagnostics = node.Diagnostics.IsDefault ?
			ImmutableArray.Create(diagnostic) :
			node.Diagnostics.Add(diagnostic);
		return node with { Diagnostics = newDiagnostics };
	}
	/// <summary>
	/// Adds a collection of diagnostics to a <see cref="SyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TNode">The type of the syntax node.</typeparam>
	/// <param name="node">The source node.</param>
	/// <param name="diagnostics">The collection of diagnostics to add.</param>
	/// <returns>A new syntax node of type <typeparamref name="TNode"/>
	/// with <paramref name="diagnostics"/> added to its diagnostics.</returns>
	public static TNode AddDiagnostic<TNode>(this TNode node, IEnumerable<Diagnostic> diagnostics) where TNode : SyntaxNode {
		var newDiagnostics = node.Diagnostics.IsDefault ?
			ImmutableArray.CreateRange(diagnostics) :
			node.Diagnostics.AddRange(diagnostics);
		return node with { Diagnostics = newDiagnostics };
	}

}
