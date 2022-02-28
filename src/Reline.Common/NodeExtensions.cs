using System.Collections.Generic;
using System.Linq;

namespace Reline.Common;

public static class NodeExtensions {

	/// <summary>
	/// Recursively gets all the child nodes of a <see cref="INode{TSelf}"/>.
	/// </summary>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>An ordered collection of descendant nodes of <paramref name="node"/>.</returns>
	public static IEnumerable<INode<TNode>> GetAllDescendants<TNode>(this INode<TNode> node) where TNode : INode<TNode> =>
		node.GetChildren().SelectMany(n => n.GetAllDescendants().Prepend(n));

}
