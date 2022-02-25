using System.Collections;
using System.Runtime.CompilerServices;
using Reline.Compilation.Syntax;

namespace Reline.Compilation.Parsing;

internal sealed class TokenViewer : IViewer<SyntaxToken> {

	private int position;



	public ImmutableArray<SyntaxToken> Tokens { get; }
	public int Position => position;
	public SyntaxToken Current => GetAt(position);
	public SyntaxToken Next => GetAt(position + 1);
	public SyntaxToken Previous => GetAt(position - 1);
	public bool IsAtEnd => position >= Tokens.Length;



	public TokenViewer(ImmutableArray<SyntaxToken> tokens) {
		Tokens = tokens;
		position = 0;
	}



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private SyntaxToken GetAt(int position) =>
		position < Tokens.Length && position >= 0 ? Tokens[position] : default;

	public void Advance() =>
		position++;
	public SyntaxToken Ahead(int distance) =>
		GetAt(Position + distance);


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
	/// Checks the type of the current token against an expected syntax type.
	/// </summary>
	/// <param name="expected">The expected <see cref="SyntaxType"/>.</param>
	/// <returns>Whether the type of the current token
	/// is equal to <paramref name="expected"/>.</returns>
	public bool CheckType(SyntaxType expected) =>
		expected == Current.Type;
	/// <summary>
	/// Checks the type of the current token against an array of expected syntax types.
	/// </summary>
	/// <param name="expected">The expected syntax types.</param>
	/// <returns>Whether the type of the current token
	/// is equal to any syntax type in <paramref name="expected"/>.</returns>
	public bool CheckType(params SyntaxType[] expected) {
		foreach (var type in expected)
			if (Current.Type == type) return true;
		return false;
	}
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
