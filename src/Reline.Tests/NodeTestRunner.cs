namespace Reline.Tests;

/// <summary>
/// Runner for tree node tests.
/// </summary>
/// <typeparam name="TNode">The tyep of </typeparam>
public sealed class NodeTestRunner<TNode> where TNode : INode<TNode> {

	private readonly IEnumerator<TNode> nodeEnumerator;



	/// <summary>
	/// Initializes a new <see cref="NodeTestRunner{TNode}"/> instance.
	/// </summary>
	/// <param name="tree">The nodes in the tree.</param>
	public NodeTestRunner(IEnumerable<TNode> tree) {
		nodeEnumerator = tree.GetEnumerator();
	}



	/// <summary>
	/// Asserts that the current node is of a specified type.
	/// </summary>
	/// <typeparam name="T">The type of the node to assert.</typeparam>
	/// <returns>The current node as <typeparamref name="T"/>.</returns>
	public T Node<T>() where T : TNode {
		Assert.True(nodeEnumerator.MoveNext(), "The tree contained no more nodes.");
		var current = nodeEnumerator.Current;
		Assert.NotNull(current);
		Assert.IsType<T>(current);
		return (T)current!;
	}
	/// <summary>
	/// Asserts that there are no more nodes in the tree.
	/// </summary>
	public void End() =>
		Assert.False(nodeEnumerator.MoveNext(), "The tree contained more nodes.");

}
