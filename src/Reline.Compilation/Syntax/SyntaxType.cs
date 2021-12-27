namespace Reline.Compilation.Syntax;

/// <summary>
/// Defines the type of a <see cref="SyntaxToken"/>.
/// </summary>
public enum SyntaxType {
	// Single-character tokens
	/// <summary>
	/// '<c>.</c>'
	/// </summary>
	DotToken,
	/// <summary>
	/// '<c>,</c>'
	/// </summary>
	CommaToken,
	/// <summary>
	/// '<c>:</c>'
	/// </summary>
	ColonToken,
	/// <summary>
	/// '<c>=</c>'
	/// </summary>
	EqualsToken,
	/// <summary>
	/// '<c>&gt;</c>'
	/// </summary>
	GreaterThanToken,
	/// <summary>
	/// '<c>&lt;</c>'
	/// </summary>
	LesserThanToken,
	/// <summary>
	/// '<c>+</c>'
	/// </summary>
	PlusToken,
	/// <summary>
	/// '<c>-</c>'
	/// </summary>
	MinusToken,
	/// <summary>
	/// '<c>*</c>'
	/// </summary>
	StarToken,
	/// <summary>
	/// '<c>/</c>'
	/// </summary>
	SlashToken,
	/// <summary>
	/// '<c>\</c>'
	/// </summary>
	BackslashToken,
	/// <summary>
	/// '<c>%</c>'
	/// </summary>
	PercentToken,
	/// <summary>
	/// '<c>(</c>'
	/// </summary>
	OpenBracketToken,
	/// <summary>
	/// '<c>)</c>'
	/// </summary>
	CloseBracketToken,
	/// <summary>
	/// '<c>[</c>'
	/// </summary>
	OpenSquareToken,
	/// <summary>
	/// '<c>]</c>'
	/// </summary>
	CloseSquareToken,
	/// <summary>
	/// '<c>{</c>'
	/// </summary>
	OpenBraceToken,
	/// <summary>
	/// '<c>}</c>'
	/// </summary>
	CloseBraceToken,
	/// <summary>
	/// Newline character.
	/// </summary>
	Newline,

	// Compound character tokens
	/// <summary>
	/// '<c>.</c>'
	/// </summary>
	DotDotToken,

	// Keywords
	/// <summary>
	/// '<c>here</c>'
	/// </summary>
	HereKeyword,
	/// <summary>
	/// '<c>start</c>'
	/// </summary>
	StartKeyword,
	/// <summary>
	/// '<c>end</c>'
	/// </summary>
	EndKeyword,
	/// <summary>
	/// '<c>move</c>'
	/// </summary>
	MoveKeyword,
	/// <summary>
	/// '<c>swap</c>'
	/// </summary>
	SwapKeyword,
	/// <summary>
	/// '<c>copy</c>'
	/// </summary>
	CopyKeyword,
	/// <summary>
	/// '<c>to</c>'
	/// </summary>
	ToKeyword,
	/// <summary>
	/// '<c>with</c>'
	/// </summary>
	WithKeyword,

	// Literals
	/// <summary>
	/// A numeric literal.
	/// </summary>
	NumberLiteral,
	/// <summary>
	/// A string literal.
	/// </summary>
	StringLiteral,
	/// <summary>
	/// An identifier.
	/// </summary>
	Identifier,

	// Special
	/// <summary>
	/// Whitespace exluding newline.
	/// </summary>
	Whitespace,
	/// <summary>
	/// A comment.
	/// </summary>
	Comment,
	/// <summary>
	/// An unknown type of syntax. Used for errors.
	/// </summary>
	Unknown,
	/// <summary>
	/// End of file.
	/// </summary>
	EndOfFile
}
