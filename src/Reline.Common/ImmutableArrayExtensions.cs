using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Reline.Common;

public static class ImmutableArrayExtensions {

	/// <summary>
	/// Returns a properly typed <see cref="IEnumerator{T}"/>
	/// from an <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the immutable array.</typeparam>
	/// <param name="immutableArray">The source immutable array.</param>
	/// <returns>An <see cref="IEnumerator{T}"/> of <paramref name="immutableArray"/>.</returns>
	public static IEnumerator<T> GetTypedEnumerator<T>(this ImmutableArray<T> immutableArray) {
		foreach (var element in immutableArray) yield return element;
	}

	/// <summary>
	/// Adds a value to an <see cref="ImmutableArray{T}"/> if a specified condition is met.
	/// </summary>
	/// <typeparam name="T">The type of the values in the immutable array.</typeparam>
	/// <param name="immutableArray">The source immutable array.</param>
	/// <param name="value">The value to add.</param>
	/// <param name="condition">The condition to check whether to add the value or not.</param>
	/// <returns>A new <see cref="ImmutableArray{T}"/> with <paramref name="value"/>
	/// added to it if <paramref name="condition"/> was <see langword="true"/>.</returns>
	public static ImmutableArray<T> AddIf<T>(this ImmutableArray<T> immutableArray, T? value, bool condition) =>
		condition ? immutableArray.Add(value!) : immutableArray;
	/// <summary>
	/// Adds a value to an <see cref="ImmutableArray{T}"/> if it is not <see langword="null"/>.
	/// </summary>
	/// <typeparam name="T">The type of the values in the immutable array.</typeparam>
	/// <param name="immutableArray">The source immutable array.</param>
	/// <param name="value">The value to add.</param>
	/// <returns>A new <see cref="ImmutableArray{T}"/> with <paramref name="value"/>
	/// added to it if <paramref name="value"/> was not <see langword="null"/>.</returns>
	public static ImmutableArray<T> AddNotNull<T>(this ImmutableArray<T> immutableArray, T? value) =>
		value is not null ? immutableArray.Add(value!) : immutableArray;

}
