namespace Reline.Compilation.Symbols;

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
	Range,
}
