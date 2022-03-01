using System.Collections.Generic;

namespace Reline.Common;

/// <summary>
/// Represents a node in a tree.
/// </summary>
/// <typeparam name="T">The type of the nodes in the tree.</typeparam>
public interface INode<T> where T : INode<T> {

	/// <summary>
	/// Gets the children of the node.
	/// </summary>
	/// <returns>A collection of children nodes.</returns>
	IEnumerable<T> GetChildren();

}
