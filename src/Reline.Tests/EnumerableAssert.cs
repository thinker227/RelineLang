namespace Reline.Tests;

/// <summary>
/// Asserts the elements of an enumerable.
/// </summary>
/// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
public sealed class EnumerableAssert<T> {

	private readonly IEnumerator<T?> enumerator;



	/// <summary>
	/// Initializes a new <see cref="EnumerableAssert{T}"/> instance.
	/// </summary>
	/// <param name="enumerable">The enumerable to assert.</param>
	public EnumerableAssert(IEnumerable<T?> enumerable) {
		enumerator = enumerable.GetEnumerator();
	}



	private void MoveNext() {
		Assert.True(enumerator.MoveNext(), "Enumerable contained no more elements.");
		Assert.NotNull(enumerator.Current);
	}
	/// <summary>
	/// Performs an action on the current element in the enumerable
	/// and moves the current element forward.
	/// </summary>
	/// <param name="action">The action to perform.</param>
	/// <returns>The current <see cref="EnumerableAssert{T}"/>.</returns>
	public EnumerableAssert<T> ElementIs(Action<T> action) {
		MoveNext();
		action(enumerator.Current!);
		return this;
	}
	/// <summary>
	/// Asserts that the current element of the enumerable is equal to
	/// another object of the same type.
	/// </summary>
	/// <param name="other">The object to equate the current element to.</param>
	/// <returns>The current <see cref="EnumerableAssert{T}"/>.</returns>
	public EnumerableAssert<T> ElementIs(T other) {
		MoveNext();
		Assert.Equal(other, enumerator.Current);
		return this;
	}
	/// <summary>
	/// Assers that there are no more elements in the enumerable.
	/// </summary>
	public void End() =>
		Assert.False(enumerator.MoveNext(), "Enumerable contained more elements.");

}

public static class EnumerableAssert {

	/// <summary>
	/// Creates an <see cref="EnumerableAssert{T}"/> from an <see cref="IEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="T">The elements in the enumerable.</typeparam>
	/// <param name="enumerable">The source enumerable.</param>
	/// <returns>A new <see cref="EnumerableAssert{T}"/>.</returns>
	public static EnumerableAssert<T> AssertEnumerable<T>(this IEnumerable<T> enumerable) =>
		new(enumerable);

}
