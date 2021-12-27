using System;

namespace Reline.Common.Text;

public static class TextSpanExtensions {

	/// <summary>
	/// Retrieves a substring from a <see cref="string"/>.
	/// </summary>
	/// <param name="str">The source string.</param>
	/// <param name="textSpan">The text span to retrieve from <paramref name="str"/>.</param>
	/// <returns>The substring <paramref name="textSpan"/> of <paramref name="str"/>.</returns>
	public static string Substring(this string str, TextSpan textSpan) =>
		str.Substring(textSpan.Start, textSpan.Length);
	/// <summary>
	/// Forms a slice out of a <see cref="ReadOnlySpan{T}"/> of characters.
	/// </summary>
	/// <param name="span">The source span.</param>
	/// <param name="textSpan">The text span to retrieve from <paramref name="span"/>.</param>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> of characters that consists of
	/// the characters of <paramref name="span"/> within <paramref name="textSpan"/>.</returns>
	public static ReadOnlySpan<char> Slice(this ReadOnlySpan<char> span, TextSpan textSpan) =>
		span.Slice(textSpan.Start, textSpan.Length);

}
