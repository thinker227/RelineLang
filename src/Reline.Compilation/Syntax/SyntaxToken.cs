namespace Reline.Compilation.Syntax;

/// <summary>
/// Represents a syntactic token.
/// </summary>
/// <param name="Type">The <see cref="SyntaxType"/> of the token.</param>
/// <param name="Position">The position of the token in the source text.</param>
/// <param name="Text">The text of the token.</param>
/// <param name="Literal">The literal value of the token.</param>
public readonly record struct SyntaxToken(
	SyntaxType Type,
	TextSpan Span,
	string Text,
	object? Literal
);
