namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents the type of a value.
/// </summary>
/// <remarks>
/// This is not strictly the same as <see cref="LiteralType"/>
/// as literals cannot be of type <see cref="Mixed"/> and bit flags
/// don't make sense for the type of a literal.
/// </remarks>
[Flags]
public enum ValueType {
	/// <summary>
	/// No type.
	/// </summary>
	None = 0,
	/// <summary>
	/// A number.
	/// </summary>
	Number = 1,
	/// <summary>
	/// A string.
	/// </summary>
	String = 2,
	/// <summary>
	/// A range.
	/// </summary>
	Range = 4,
	/// <summary>
	/// A mixed type.
	/// The type of variables, parameters and function invocations.
	/// </summary>
	Mixed = 8,

	/// <summary>
	/// A constant type.
	/// </summary>
	Constant = Number | String | Range,
	/// <summary>
	/// Any type.
	/// </summary>
	Any = Number | String | Range | Mixed,
}
