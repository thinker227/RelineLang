namespace Reline.Tests;

public static class ObjectExtensions {

	/// <summary>
	/// Performs an action on an object.
	/// </summary>
	/// <typeparam name="T">The type of the object to perform the action on.</typeparam>
	/// <param name="obj">The object to perform the action on.</param>
	/// <param name="action">The action to perform.</param>
	/// <returns><paramref name="obj"/>.</returns>
	public static T Do<T>(this T obj, Action<T> action) {
		action(obj);
		return obj;
	}
	/// <summary>
	/// Asserts that an object is of a specified type and returns the object as the type.
	/// </summary>
	/// <typeparam name="T">The type to assert the object as.</typeparam>
	/// <param name="obj">The object to assert the type of.</param>
	/// <returns><paramref name="obj"/> as <typeparamref name="T"/>.</returns>
	public static T IsType<T>(this object? obj) {
		Assert.NotNull(obj);
		Assert.IsType<T>(obj);
		return (T)obj!;
	}
	/// <summary>
	/// Asserts that an object is equal to another object of the same type.
	/// </summary>
	/// <typeparam name="T">The type of the objects to assert.</typeparam>
	/// <param name="obj">The object to assert.</param>
	/// <param name="other">The object to assert equality to.</param>
	/// <returns><paramref name="obj"/>.</returns>
	public static T IsEqualTo<T>(this T obj, T other) {
		Assert.Equal(obj, other);
		return obj;
	}

}
