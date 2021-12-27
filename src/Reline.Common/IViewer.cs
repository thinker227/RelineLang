using System.Collections.Generic;

namespace Reline.Common;

/// <summary>
/// Represents a viewer of a sequence.
/// </summary>
public interface IViewer {

	/// <summary>
	/// Whether the viewer is at the end of the sequence.
	/// </summary>
	bool IsAtEnd { get; }

	/// <summary>
	/// Advances the viewer forward by one element.
	/// </summary>
	void Advance();

}

/// <summary>
/// Represents a viewer of a generic sequence.
/// </summary>
/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
public interface IViewer<out T> : IViewer, IEnumerable<T> {

	/// <summary>
	/// The current element.
	/// </summary>
	T Current { get; }
	/// <summary>
	/// The next element.
	/// </summary>
	T Next { get; }

	/// <summary>
	/// Gets the element a specified distance away.
	/// </summary>
	/// <param name="distance">The distance away to get the element at.</param>
	/// <returns>The element <paramref name="distance"/> elements away.</returns>
	T Ahead(int distance);

}
