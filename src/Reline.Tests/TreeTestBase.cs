using System.Linq;
using Reline.Common;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;
using Xunit.Sdk;

namespace Reline.Tests;

/// <summary>
/// Base for testing trees.
/// </summary>
/// <typeparam name="TNode">The type of node the class tests.</typeparam>
public abstract class TreeTestBase<TNode> : TestBase where TNode : INode<TNode> {

	private IEnumerator<TNode>? nodeEnumerator;



	/// <summary>
	/// Gets an enumerable to enumerate over the tree.
	/// </summary>
	/// <returns>An <see cref="IEnumerable{T}"/> of the nodes in the tree.</returns>
	protected abstract IEnumerable<TNode>? GetEnumerable();
	/// <summary>
	/// Resets the state of the tree.
	/// </summary>
	protected abstract void Reset();

	/// <summary>
	/// Asserts that the current node is of a specified type.
	/// </summary>
	/// <typeparam name="T">The type of the node to assert.</typeparam>
	/// <returns>The current node as <typeparamref name="T"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <see cref="SetTree(string)"/> has not been called
	/// and the tree to enumerate is <see langword="null"/>.
	/// </exception>
	protected T Node<T>() where T : TNode {
		if (nodeEnumerator is null) {
			nodeEnumerator =
				(GetEnumerable() ?? throw new InvalidOperationException("No tree to enumerate."))
				.GetEnumerator();
		}

		try {
			Assert.True(nodeEnumerator.MoveNext(), "The tree contained no more nodes.");
			var current = nodeEnumerator.Current;
			Assert.NotNull(current);
			Assert.IsType<T>(current);
			return (T)current!;
		} catch (XunitException e) {
			nodeEnumerator = null;
			throw e;
		}
	}
	/// <summary>
	/// Asserts that there are no more nodes in the tree.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// <see cref="SetTree(string)"/> has not been called
	/// and the tree to enumerate is <see langword="null"/>.
	/// </exception>
	protected void End() {
		if (nodeEnumerator is null)
			throw new InvalidOperationException($"No tree to enumerate.");

		try {
			Assert.False(nodeEnumerator.MoveNext(), "The tree contained more nodes.");
		} finally {
			nodeEnumerator = null;
			Reset();
		}
	}

}
