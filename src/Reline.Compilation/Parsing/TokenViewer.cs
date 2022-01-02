using System.Linq;
using System.Collections;
using System.Runtime.CompilerServices;
using Reline.Compilation.Syntax;

namespace Reline.Compilation.Parsing;

internal sealed class TokenViewer : IViewer<SyntaxToken> {

	private int position;



	public ImmutableArray<SyntaxToken> Tokens { get; }
	public SyntaxToken Current => GetAt(position);
	public SyntaxToken Next => NextNotWhitespace(1);
	public SyntaxToken Previous => NextNotWhitespace(-1);
	public bool IsAtEnd => position >= Tokens.Length;



	public TokenViewer(ImmutableArray<SyntaxToken> tokens) {
		Tokens = tokens;
		position = 0;

		ExpectNotWhitespace();
	}



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private SyntaxToken GetAt(int position) =>
		position < Tokens.Length ? Tokens[position] : default;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private SyntaxToken NextNotWhitespace(int direction) {
		int i = position;
		while (true) {
			i += direction;
			var current = GetAt(i);
			if (SyntaxRules.IsWhitespaceLike(current.Type)) continue;
			return current;
		}
	}

	public void Advance() {
		do position++;
		while (Current.Type == SyntaxType.Whitespace);
	}
	public SyntaxToken Ahead(int distance) {
		int remaining = distance;
		int i = 0;
		SyntaxToken current = default;
		while (remaining > 0) {
			i++;
			current = GetAt(position + i);
			if (current.Type != SyntaxType.Whitespace) remaining--;
		}
		return current;
	}


	/// <summary>
	/// Advances the viewer to the next token of a specified type.
	/// </summary>
	/// <param name="type">The type to expect.</param>
	public SyntaxToken ExpectType(SyntaxType type) {
		do position++;
		while (Current.Type != type);
		return Current;
	}
	/// <summary>
	/// Advances the viewer to the next token of any of an array specified types.
	/// </summary>
	/// <param name="types">The types to expect.</param>
	public SyntaxToken ExpectType(params SyntaxType[] types) {
		do position++;
		while (!types.Contains(Current.Type));
		return Current;
	}
	/// <summary>
	/// Advances the viewer to the next token not matching
	/// <see cref="SyntaxRules.IsWhitespaceLike(SyntaxType)"/>.
	/// </summary>
	public SyntaxToken ExpectNotWhitespace() {
		while (SyntaxRules.IsWhitespaceLike(Current.Type)) Advance();
		return Current;
	}

	public bool CheckType(SyntaxType expected) =>
		expected == Current.Type;
	public bool CheckType(params SyntaxType[] expected) {
		foreach (var type in expected)
			if (Current.Type == type) return true;
		return false;
	}
	public bool CheckType(Predicate<SyntaxType> predicate) =>
		predicate(Current.Type);
	/// <summary>
	/// Matches a type pattern again the types of the tokens
	/// from and including the current token, not including whitespace.
	/// </summary>
	/// <param name="types">The type pattern to match.</param>
	/// <returns>Whether <paramref name="types"/> matches the types of the
	/// immediately following tokens including the current token,
	/// excluding tokens matching
	/// <see cref="SyntaxRules.IsWhitespaceLike(SyntaxType)"/>.</returns>
	public bool MatchTypePattern(params SyntaxType[] types) {
		int currentPos = position - 1;
		int currentMatch = 0;

		while (currentMatch < types.Length) {
			currentPos++;
			var current = GetAt(currentPos);
			if (SyntaxRules.IsWhitespaceLike(current.Type)) continue;
			if (current.Type != types[currentMatch]) return false;
			currentMatch++;
		}

		return true;
	}

	public SyntaxToken[] GetLeadingWhitespace() {
		List<SyntaxToken> tokens = new();
		int i = 0;
		while (true) {
			i--;
			var current = GetAt(position - i);
			if (current.Type != SyntaxType.Whitespace) return tokens.ToArray();
			tokens.Add(current);
		}
	}
	public SyntaxToken[] GetTrailingWhitespace() {
		List<SyntaxToken> tokens = new();
		int i = 0;
		while (true) {
			i++;
			var current = GetAt(position - i);
			if (current.Type != SyntaxType.Whitespace) return tokens.ToArray();
			tokens.Add(current);
		}
	}

	public IEnumerator<SyntaxToken> GetEnumerator() =>
		Tokens.AsEnumerable().GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

}
