﻿using Reline.Common;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;
using Xunit.Sdk;

namespace Reline.Tests;

public abstract class ParserTestBase : TestBase {

	private IEnumerator<ISyntaxNode>? nodeEnumerator;



	/// <summary>
	/// Sets the tree to traverse from a source string.
	/// </summary>
	/// <param name="source">The source stirng to generate the tree from.</param>
	protected void SetTree(string source) {
		var tree = AssertAsync.CompletesIn(2000, () => Parser.ParseString(source));
		var nodes = tree.Root.GetAllDescendants();
		nodeEnumerator = nodes.AsEnumerable().GetEnumerator();
	}

	/// <summary>
	/// Asserts that the current node is of a specified type.
	/// </summary>
	/// <typeparam name="TNode">The type of the node to assert.</typeparam>
	/// <returns>The current node as <typeparamref name="TNode"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// <see cref="SetTree(string)"/> has not been called
	/// and the tree to enumerate is <see langword="null"/>.
	/// </exception>
	protected TNode Node<TNode>() where TNode : ISyntaxNode {
		if (nodeEnumerator is null)
			throw new InvalidOperationException($"{nameof(SetTree)} must be called before {nameof(Node)}.");

		try {
			Assert.True(nodeEnumerator.MoveNext());
			var current = nodeEnumerator.Current;
			Assert.IsType<TNode>(current);
			return (TNode)current;
		} catch (XunitException e) {
			nodeEnumerator = null;
			throw e;
		}
	}

}
