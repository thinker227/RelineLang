using System.Diagnostics.CodeAnalysis;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax;

public static class SyntaxTokenExtensions {

	/// <summary>
	/// Tries to get the literal value of a <see cref="SyntaxToken"/> as a specified type.
	/// </summary>
	/// <typeparam name="T">The type to try get the literal value as.</typeparam>
	/// <param name="value">The value of <see cref="SyntaxToken.Literal"/>
	/// as <typeparamref name="T"/>.</param>
	/// <returns>Whether the literal value could be converted to
	/// <typeparamref name="T"/>.</returns>
	public static bool TryGetLiteralAs<T>(this SyntaxToken token, [NotNullWhen(true)] out T? value) {
		if (token.Literal is T asT) {
			value = asT;
			return true;
		}

		value = default;
		return false;
	}

	/// <summary>
	/// Returns a new <see cref="SyntaxToken"/> with specified leading trivia.
	/// </summary>
	/// <param name="token">The source syntax token.</param>
	/// <param name="trivia">The trivia to add.</param>
	/// <returns>A new <see cref="SyntaxToken"/> with <paramref name="trivia"/> as
	/// the <see cref="SyntaxToken.LeadingTrivia"/>.</returns>
	public static SyntaxToken WithLeadingTrivia(this SyntaxToken token, IEnumerable<SyntaxTrivia> trivia) =>
		token with { LeadingTrivia = trivia.ToImmutableArray() };
	/// <summary>
	/// Returns a new <see cref="SyntaxToken"/> with specified trailing trivia.
	/// </summary>
	/// <param name="token">The source syntax token.</param>
	/// <param name="trivia">The trivia to add.</param>
	/// <returns>A new <see cref="SyntaxToken"/> with <paramref name="trivia"/> as
	/// the <see cref="SyntaxToken.TrailingTrivia"/>.</returns>
	public static SyntaxToken WithTrailingTrivia(this SyntaxToken token, IEnumerable<SyntaxTrivia> trivia) =>
		token with { TrailingTrivia = trivia.ToImmutableArray() };

	/// <summary>
	/// Converts a <see cref="SyntaxToken"/> to a <see cref="SyntaxTrivia"/>.
	/// </summary>
	/// <param name="token">The token to convert.</param>
	/// <returns>A new <see cref="SyntaxTrivia"/> instance
	/// constructed from <paramref name="token"/>.</returns>
	public static SyntaxTrivia ToSyntaxTrivia(this SyntaxToken token) =>
		new(token.Span, token.Text);

}
