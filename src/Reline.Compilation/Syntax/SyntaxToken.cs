using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax;

/// <summary>
/// Represents a syntactic token.
/// </summary>
/// <param name="Type">The <see cref="SyntaxType"/> of the token.</param>
/// <param name="Span">The span of the token in the source text.</param>
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
	public ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; } =
		ImmutableArray<SyntaxTrivia>.Empty;
	/// <summary>
	/// The trailing trivia of the token.
	/// </summary>
	public ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; } =
		ImmutableArray<SyntaxTrivia>.Empty;
	/// <summary>
	/// The diagnostics of the token.
	/// </summary>
	public ImmutableArray<Diagnostic> Diagnostics { get; init; } =
		ImmutableArray<Diagnostic>.Empty;
	/// <summary>
	/// Whether the token is missing in the source text.
	/// </summary>
	public bool IsMissing =>
		Type == SyntaxType.Unknown || Span.IsEmpty;



	public override string ToString() {
		string literalString = Literal is null ? "" : $" ({Literal})";
		string textString = Span.IsEmpty ? "" : $" {Span} \"{Text}\"";
		return $"{Type}{literalString}{textString}";
	}

}
