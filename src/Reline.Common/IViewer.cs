using System.Collections.Generic;

namespace Reline.Common;

/// <summary>
/// Represents a viewer of a sequence.
/// </summary>
/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
public interface IViewer<out T> : IEnumerable<T> {

	/// <summary>
	/// The current element.
	/// </summary>
	T Current { get; }
	/// <summary>
	/// The next element.
	/// </summary>
	T Next { get; }
	/// <summary>
	/// Whether the viewer is at the end of the sequence.
	/// </summary>
	bool IsAtEnd { get; }

	/// <summary>
	/// Moves the viewer forward one element.
	/// </summary>
	void MoveNext();

}
