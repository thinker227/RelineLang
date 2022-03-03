using System.Diagnostics.CodeAnalysis;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a literal value.
/// </summary>
public readonly struct BoundValue : IEquatable<BoundValue> {

	/// <summary>
	/// The value of the literal.
	/// </summary>
	public object Value { get; }
	/// <summary>
	/// The type of the literal.
	/// </summary>
	public BoundValueType Type { get; }



	/// <summary>
	/// Initializes a new <see cref="BoundValue"/> instance.
	/// </summary>
	/// <param name="int">The number value of the bound value.</param>
	public BoundValue(int @int) {
		Value = @int;
		Type = BoundValueType.Number;
	}
	/// <summary>
	/// Initializes a new <see cref="BoundValue"/> instance.
	/// </summary>
	/// <param name="string">The string value of the bound value.</param>
	public BoundValue(string @string) {
		Value = @string;
		Type = BoundValueType.String;
	}
	/// <summary>
	/// Initializes a new <see cref="BoundValue"/> instance.
	/// </summary>
	/// <param name="range">The range value of the bound value.</param>
	public BoundValue(RangeValue range) {
		Value = range;
		Type = BoundValueType.Range;
	}



	/// <summary>
	/// Switches over the value of the bound value.
	/// </summary>
	/// <param name="int">The <see cref="Action{T}"/>
	/// to invoke if the bound value is a number.</param>
	/// <param name="string">The <see cref="Action{T}"/>
	/// to invoke if the bound value is a string.</param>
	/// <param name="range">The <see cref="Action{T}"/>
	/// to invoke if the bound value is a range.</param>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Type"/> is <see cref="BoundValueType.None"/>.
	/// </exception>
	public void Switch(Action<int> @int, Action<string> @string, Action<RangeValue> range) {
		switch (Type) {
			case BoundValueType.Number: @int((int)Value); break;
			case BoundValueType.String: @string((string)Value); break;
			case BoundValueType.Range: range((RangeValue)Value); break;
			default: throw new InvalidOperationException();
		}
	}
	/// <summary>
	/// Switches over the value of the bound value.
	/// </summary>
	/// <typeparam name="TResult">The return type of the invoked function.</typeparam>
	/// <param name="int">The <see cref="Func{T, TResult}"/>
	/// to invoke if the bound value is a number.</param>
	/// <param name="string">The <see cref="Func{T, TResult}"/>
	/// to invoke if the bound value is a string.</param>
	/// <param name="range">The <see cref="Func{T, TResult}"/>
	/// to invoke if the bound value is a range.</param>
	/// <returns>The return value of the invoked function.</returns>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Type"/> is <see cref="BoundValueType.None"/>.
	/// </exception>
	public TResult Switch<TResult>(Func<int, TResult> @int, Func<string, TResult> @string, Func<RangeValue, TResult> range) =>
	Type switch {
		BoundValueType.Number => @int((int)Value),
		BoundValueType.String => @string((string)Value),
		BoundValueType.Range => range((RangeValue)Value),
		_ => throw new InvalidOperationException()
	};
	public T GetAs<T>() =>
		(T)Value;
	/// <summary>
	/// Tries to get <see cref="Value"/> as a specified type.
	/// </summary>
	/// <typeparam name="T">The type to get <see cref="Value"/> as.</typeparam>
	/// <param name="value"><see cref="Value"/> as <typeparamref name="T"/>
	/// or <see langword="default"/> if <see cref="Value"/>
	/// is not of type <typeparamref name="T"/>.</param>
	/// <returns>Whether <see cref="Value"/> is of type <typeparamref name="T"/>.</returns>
	public bool TryGetAs<T>([NotNullWhen(true)] out T? value) {
		if (Value is T t) {
			value = t;
			return true;
		}

		value = default;
		return false;
	}

	public bool Equals(BoundValue other) =>
		Value?.Equals(other.Value) ?? other.Value is null;
	public override bool Equals(object? obj) =>
		obj is BoundValue other && Equals(other);
	public override int GetHashCode() =>
		Value.GetHashCode();
	public override string ToString() {
		if (Type == BoundValueType.None) return "none";
		string typeString = Type switch {
			BoundValueType.Number => "number",
			BoundValueType.String => "string",
			_ => ""
		};
		return $"{Value} ({typeString})";
	}



	public static bool operator ==(BoundValue a, BoundValue b) =>
		a.Equals(b);
	public static bool operator !=(BoundValue a, BoundValue b) =>
		!a.Equals(b);

	public static implicit operator BoundValue(int @int) =>
		new(@int);
	public static implicit operator BoundValue(string @string) =>
		new(@string);
	public static implicit operator BoundValue(RangeValue range) =>
		new(range);

}
