using System.Diagnostics.CodeAnalysis;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a literal value.
/// </summary>
public readonly struct LiteralValue : IEquatable<LiteralValue> {

	/// <summary>
	/// The value of the literal.
	/// </summary>
	public object Value { get; }
	/// <summary>
	/// The type of the literal.
	/// </summary>
	public LiteralType Type { get; }



	/// <summary>
	/// Initializes a new <see cref="LiteralValue"/> instance.
	/// </summary>
	/// <param name="int">The number value of the literal.</param>
	public LiteralValue(int @int) {
		Value = @int;
		Type = LiteralType.Number;
	}
	/// <summary>
	/// Initializes a new <see cref="LiteralValue"/> instance.
	/// </summary>
	/// <param name="string">The string value of the literal.</param>
	public LiteralValue(string @string) {
		Value = @string;
		Type = LiteralType.String;
	}
	/// <summary>
	/// Initializes a new <see cref="LiteralValue"/> instance.
	/// </summary>
	/// <param name="range">The range value of the literal.</param>
	public LiteralValue(RangeLiteral range) {
		Value = range;
		Type = LiteralType.Range;
	}



	/// <summary>
	/// Switches over the value of the literal.
	/// </summary>
	/// <param name="int">The <see cref="Action{T}"/>
	/// to invoke if the literal is a number.</param>
	/// <param name="string">The <see cref="Action{T}"/>
	/// to invoke if the literal is a string.</param>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Type"/> is <see cref="LiteralType.None"/>.
	/// </exception>
	public void Switch(Action<int> @int, Action<string> @string, Action<RangeLiteral> range) {
		switch (Type) {
			case LiteralType.Number: @int((int)Value); break;
			case LiteralType.String: @string((string)Value); break;
			case LiteralType.Range: range((RangeLiteral)Value); break;
			default: throw new InvalidOperationException();
		}
	}
	/// <summary>
	/// Switches over the value of the literal.
	/// </summary>
	/// <typeparam name="TResult">The return type of the invoked function.</typeparam>
	/// <param name="int">The <see cref="Func{T, TResult}"/>
	/// to invoke if the literal is a number.</param>
	/// <param name="string">The <see cref="Func{T, TResult}"/>
	/// to invoke if the literal is a string.</param>
	/// <returns>The return value of the invoked function.</returns>
	/// <exception cref="InvalidOperationException">
	/// <see cref="Type"/> is <see cref="LiteralType.None"/>.
	/// </exception>
	public TResult Switch<TResult>(Func<int, TResult> @int, Func<string, TResult> @string, Func<RangeLiteral, TResult> range) =>
	Type switch {
		LiteralType.Number => @int((int)Value),
		LiteralType.String => @string((string)Value),
		LiteralType.Range => range((RangeLiteral)Value),
		_ => throw new InvalidOperationException()
	};
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

	public bool Equals(LiteralValue other) =>
		Value?.Equals(other.Value) ?? other.Value is null;
	public override bool Equals(object? obj) =>
		obj is LiteralValue other && Equals(other);
	public override int GetHashCode() =>
		Value.GetHashCode();
	public override string ToString() {
		if (Type == LiteralType.None) return "none";
		string typeString = Type switch {
			LiteralType.Number => "number",
			LiteralType.String => "string",
			_ => ""
		};
		return $"{Value} ({typeString})";
	}



	public static bool operator ==(LiteralValue a, LiteralValue b) =>
		a.Equals(b);
	public static bool operator !=(LiteralValue a, LiteralValue b) =>
		!a.Equals(b);

	public static implicit operator LiteralValue(int @int) =>
		new(@int);
	public static implicit operator LiteralValue(string @string) =>
		new(@string);
	public static implicit operator LiteralValue(RangeLiteral range) =>
		new(range);

}

/// <summary>
/// Represents the type of a literal.
/// </summary>
public enum LiteralType {
	/// <summary>
	/// The literal does not have a type or is invalid.
	/// </summary>
	None,
	/// <summary>
	/// The literal is a number.
	/// </summary>
	Number,
	/// <summary>
	/// The literal is a string.
	/// </summary>
	String,
	/// <summary>
	/// The literal is a range.
	/// </summary>
	Range
}
