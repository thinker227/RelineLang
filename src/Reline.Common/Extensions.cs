using System;

namespace Reline.Common;

public static class Extensions {

	/// <summary>
	/// Calls <see cref="IViewer{T}.MoveNext"/> and returns the previous element.
	/// </summary>
	/// <typeparam name="T">The type of the <see cref="IViewer{T}"/>.</typeparam>
	/// <param name="viewer">The source viewer.</param>
	/// <returns>The current element of <paramref name="viewer"/>.</returns>
	public static T MoveNextCurrent<T>(this IViewer<T> viewer) {
		T current = viewer.Current;
		viewer.MoveNext();
		return current;
	}

	public static void MoveWhile<T>(this IViewer<T> viewer, Predicate<T> predicate) {
		while (!viewer.IsAtEnd && predicate(viewer.Current)) viewer.MoveNext();
	}

}
