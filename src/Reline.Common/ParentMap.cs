using System.Collections.Generic;
using System.Threading;

namespace Reline.Common;

/// <summary>
/// A map of nodes in a tree and their parents.
/// </summary>
/// <typeparam name="TNode">The type of the nodes in the tree.</typeparam>
public sealed class ParentMap<TNode> where TNode : INode<TNode> {

	private IReadOnlyDictionary<TNode, TNode?>? parents;
	private readonly TNode root;



	/// <summary>
	/// Initializes a new <see cref="ParentMap{TNode}"/> instance.
	/// </summary>
	/// <param name="root">The root node of the tree.</param>
	public ParentMap(TNode root) {
		this.root = root;
	}



	/// <summary>
	/// Gets the parent node of a specified <typeparamref name="TNode"/>.
	/// </summary>
	/// <param name="node">The <typeparamref name="TNode"/>
	/// to get the parent of.</param>
	/// <returns>The parent of <paramref name="node"/>, or <see langword="default"/>
	/// if the node is the root of the tree.</returns>
	public TNode? GetParent(TNode node) {
		if (parents is null) {
			var createdParents = CreateParentsDictionary(root);
			Interlocked.CompareExchange(ref parents, createdParents, null);
		}

		return parents[node];
	}
	private static IReadOnlyDictionary<TNode, TNode?> CreateParentsDictionary(TNode root) {
		Dictionary<TNode, TNode?> result = new();
		result.Add(root, default);
		AddParentsToDictionary(result, root);
		return result;
	}
	private static void AddParentsToDictionary(IDictionary<TNode, TNode?> dictionary, TNode node) {
		var children = node.GetChildren();
		foreach (var child in children) {
			dictionary.Add(child, node);
			AddParentsToDictionary(dictionary, child);
		}
	}

}
