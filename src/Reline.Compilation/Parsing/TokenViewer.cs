using System.Linq;
using System.Collections;
using Reline.Compilation.Syntax;

namespace Reline.Compilation.Parsing;

internal sealed class TokenViewer : IViewer<SyntaxToken> {

	private int position;



	public ImmutableArray<SyntaxToken> Tokens { get; }
	public SyntaxToken Current => GetAt(position);
	public SyntaxToken Next => GetAt(position + 1);
	public bool IsAtEnd => position >= Tokens.Length;



	public TokenViewer(ImmutableArray<SyntaxToken> tokens) {
		Tokens = tokens;
		position = 0;
	}



	private SyntaxToken GetAt(int position) =>
		position < Tokens.Length ? Tokens[position] : default;

	public void Advance() =>
		position++;
	/// <summary>
	/// Advances the viewer forward by one element,
	/// skipping tokens with a type of <see cref="SyntaxType.Whitespace"/>.
	/// </summary>
	public void AdvanceNotWhitespace() {
		do Advance();
		while (Current.Type == SyntaxType.Whitespace);
	}
	public void ExpectNotWhitespace() {
		while (Current.Type == SyntaxType.Whitespace) Advance();
	}
	public SyntaxToken Ahead(int distance) =>
		GetAt(position + distance);
	/// <summary>
	/// Gets the <see cref="SyntaxToken"/> a specified distance away, not counting whitespace.
	/// </summary>
	/// <param name="distance">The distance away to get the token at.</param>
	/// <returns>The <see cref="SyntaxToken"/> <paramref name="distance"/> elements away,
	/// not including tokens with a type of <see cref="SyntaxType.Whitespace"/>.</returns>
	public SyntaxToken AheadNotWhitespace(int distance) {
		int remaining = distance;
		int i = 1;
		SyntaxToken current = default;
		while (remaining > 0) {
			current = Ahead(i++);
			if (current.Type != SyntaxType.Whitespace) remaining--;
		}
		return current;
	}

	/// <summary>
	/// Matches a type pattern again the types of the tokens
	/// from and including the current token, not including whitespace.
	/// </summary>
	/// <param name="types">The type pattern to match.</param>
	/// <returns>Whether <paramref name="types"/> matches the types of the
	/// immediately following tokens including the current token,
	/// excluding tokens with a type of <see cref="SyntaxType.Whitespace"/>.</returns>
	public bool MatchTypePatternNotWhitespace(params SyntaxType[] types) {
		int currentPos = position - 1;
		int currentMatch = 0;

		while (currentMatch < types.Length) {
			currentPos++;
			var current = GetAt(currentPos);
			if (current.Type == SyntaxType.Whitespace) continue;
			if (current.Type != types[currentMatch]) return false;
			currentMatch++;
		}

		return true;
	}

	public IEnumerator<SyntaxToken> GetEnumerator() =>
		Tokens.AsEnumerable().GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

}
