namespace Reline.Compilation.Syntax;

/// <summary>
/// Represents trailing or leading syntax trivia.
/// </summary>
/// <param name="Span">The span of the trivia in the source text.</param>
/// <param name="Text">The text of the trivia.</param>
public readonly record struct SyntaxTrivia(
	TextSpan Span,
	string Text
) {

	public override string ToString() =>
		$"{Span} \"{Text}\"";

}
