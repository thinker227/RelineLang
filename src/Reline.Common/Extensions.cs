using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Reline.Common;

public static class Extensions {

	public static IEnumerable<T> AsEnumerable<T>(this ImmutableArray<T> immutableArray) {
		foreach (T element in immutableArray) yield return element;
	}

}
