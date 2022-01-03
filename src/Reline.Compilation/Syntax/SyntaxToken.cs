﻿using Reline.Compilation.Diagnostics;

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
) {

	// LeadingTrivia and TrailingTrivia cannot be immutable arrays
	/// <summary>
	/// The leading trivia of the token.
	/// </summary>
	public IReadOnlyCollection<SyntaxToken> LeadingTrivia { get; init; } =
		ImmutableArray<SyntaxToken>.Empty;
	/// <summary>
	/// The trailing trivia of the token.
	/// </summary>
	public IReadOnlyCollection<SyntaxToken> TrailingTrivia { get; init; } =
		ImmutableArray<SyntaxToken>.Empty;
	/// <summary>
	/// The diagnostics of the token.
	/// </summary>
	public ImmutableArray<Diagnostic> Diagnostics { get; init; } =
		ImmutableArray<Diagnostic>.Empty;



	public override string ToString() {
		string literalString = Literal is null ? "" : $" ({Literal})";
		string textString = Span.IsEmpty ? "" : $" {Span} \"{Text}\"";
		return $"{Type}{literalString}{textString}";
	}

}
