using System;

namespace Reline.Common;

public static class ViewerExtensions {

	/// <summary>
	/// Calls <see cref="IViewer{T}.MoveNext"/> and returns the previous element.
	/// </summary>
	/// <typeparam name="T">The type of the <see cref="IViewer{T}"/>.</typeparam>
	/// <param name="viewer">The source viewer.</param>
	/// <returns>The current element of <paramref name="viewer"/>.</returns>
	public static T MoveNextCurrent<T>(this IViewer<T> viewer) {
		T current = viewer.Current;
		viewer.Advance();
		return current;
	}

	/// <summary>
	/// Calls <see cref="IViewer{T}.MoveNext"/> while a specified predicate is true.
	/// </summary>
	/// <typeparam name="T">The type of the <see cref="IViewer{T}"/>.</typeparam>
	/// <param name="viewer">The source viewer.</param>
	/// <param name="predicate">The predicate to call on each element.</param>
	public static void MoveWhile<T>(this IViewer<T> viewer, Predicate<T> predicate) {
		while (!viewer.IsAtEnd && predicate(viewer.Current)) viewer.Advance();
	}

	/// <summary>
	/// Moves a <see cref="IViewer{T}"/> a specified distance.
	/// </summary>
	/// <typeparam name="T">The type of the <see cref="IViewer{T}"/>.</typeparam>
	/// <param name="viewer">The source viewer.</param>
	/// <param name="distance">The distance to move.</param>
	public static void MoveDistance<T>(this IViewer<T> viewer, int distance) {
		for (int i = 0; i < distance; i++) viewer.Advance();
	}

}
