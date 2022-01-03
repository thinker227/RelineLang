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
	public static SyntaxToken WithLeadingTrivia(this SyntaxToken token, IEnumerable<SyntaxToken> trivia) =>
		token with { LeadingTrivia = trivia.ToImmutableArray() };
	/// <summary>
	/// Returns a new <see cref="SyntaxToken"/> with specified trailing trivia.
	/// </summary>
	/// <param name="token">The source syntax token.</param>
	/// <param name="trivia">The trivia to add.</param>
	/// <returns>A new <see cref="SyntaxToken"/> with <paramref name="trivia"/> as
	/// the <see cref="SyntaxToken.TrailingTrivia"/>.</returns>
	public static SyntaxToken WithTrailingTrivia(this SyntaxToken token, IEnumerable<SyntaxToken> trivia) =>
		token with { TrailingTrivia = trivia.ToImmutableArray() };

	/// <summary>
	/// Adds a diagnostic to a <see cref="SyntaxToken"/>.
	/// </summary>
	/// <param name="token">The source syntax token.</param>
	/// <param name="diagnostic">The diagnostic to add.</param>
	/// <returns>A new <see cref="SyntaxToken"/> with <paramref name="diagnostic"/>
	/// added to its diagnostics.</returns>
	public static SyntaxToken AddDiagnostic(this SyntaxToken token, Diagnostic diagnostic) {
		var newDiagnostics = token.Diagnostics.IsDefault ?
			ImmutableArray.Create(diagnostic) :
			token.Diagnostics.Add(diagnostic);
		return token with { Diagnostics = newDiagnostics };
	}
	/// <summary>
	/// Adds a collection of diagnostics to a <see cref="SyntaxToken"/>.
	/// </summary>
	/// <param name="token">The source syntax token.</param>
	/// <param name="diagnostic">The diagnostics to add.</param>
	/// <returns>A new <see cref="SyntaxToken"/> with <paramref name="diagnostics"/>
	/// added to its diagnostics.</returns>
	public static SyntaxToken AddDiagnostics(this SyntaxToken token, IEnumerable<Diagnostic> diagnostics) {
		var newDiagnostics = token.Diagnostics.IsDefault ?
			ImmutableArray.CreateRange(diagnostics) :
			token.Diagnostics.AddRange(diagnostics);
		return token with { Diagnostics = newDiagnostics };
	}

	/// <summary>
	/// Returns whether a <see cref="SyntaxToken"/> is missing from the source text.
	/// </summary>
	/// <param name="token">The syntax token to check.</param>
	/// <returns>Whether the type of <paramref name="token"/> is <see cref="SyntaxType.Unknown"/> or
	/// <see cref="SyntaxToken.Span"/> is empty.</returns>
	public static bool IsMissing(this SyntaxToken token) =>
		token.Type == SyntaxType.Unknown || token.Span.IsEmpty;

}
