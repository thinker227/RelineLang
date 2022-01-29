namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents the type of a literal.
/// </summary>
[Flags]
public enum LiteralType {
	/// <summary>
	/// The literal does not have a type or is invalid.
	/// </summary>
	None = 0,
	/// <summary>
	/// The literal is a number.
	/// </summary>
	Number = 1,
	/// <summary>
	/// The literal is a string.
	/// </summary>
	String = 2,
	/// <summary>
	/// The literal is a range.
	/// </summary>
	Range = 4,
}
