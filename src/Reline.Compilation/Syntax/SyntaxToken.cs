using System.Diagnostics.CodeAnalysis;

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

	/// <summary>
	/// Tries to get <see cref="Literal"/> as a specified type.
	/// </summary>
	/// <typeparam name="T">The type to try get <see cref="Literal"/> as.</typeparam>
	/// <param name="value">The value of <see cref="Literal"/>
	/// as <typeparamref name="T"/>.</param>
	/// <returns>Whether <see cref="Literal"/> could be converted to
	/// <typeparamref name="T"/>.</returns>
	public bool TryGetLiteralAs<T>([NotNullWhen(true)] out T? value) {
		if (Literal is T asT) {
			value = asT;
			return true;
		}

		value = default;
		return false;
	}

}
