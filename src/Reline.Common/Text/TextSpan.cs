using System;

namespace Reline.Common.Text;

/// <summary>
/// Represents a span in a text.
/// </summary>
public readonly struct TextSpan : IEquatable<TextSpan> {

	/// <summary>
	/// The inclusive start index of the span.
	/// </summary>
	public int Start { get; }
	/// <summary>
	/// The exclusive end index of the span.
	/// </summary>
	public int End { get; }
	/// <summary>
	/// The length of the span.
	/// </summary>
	public int Length => End - Start;
	/// <summary>
	/// Whether the span is empty.
	/// </summary>
	public bool IsEmpty => Length == 0;

	/// <summary>
	/// An empty <see cref="TextSpan"/>.
	/// </summary>
	public static TextSpan Empty { get; } = new(0, 0);



	/// <summary>
	/// Initializes a new <see cref="TextSpan"/> instance.
	/// </summary>
	/// <param name="start">The inclusive start index of the span.</param>
	/// <param name="end">The exclusive end index of the span.</param>
	public TextSpan(int start, int end) {
		if (end < start) throw new ArgumentException("Length of text span cannot be negative.", nameof(end));
		if (start < 0) throw new ArgumentOutOfRangeException(nameof(start), "Start of text span cannot be negative.");
		
		Start = start;
		End = end;
	}



	/// <summary>
	/// Creates a <see cref="TextSpan"/> from a start index and length.
	/// </summary>
	/// <param name="start">The inclusive start index of the span.</param>
	/// <param name="length">The length of the span.</param>
	/// <returns>A new <see cref="TextSpan"/> with a start position of <paramref name="start"/> and
	/// an end position of <paramref name="start"/> offset by <paramref name="length"/>.</returns>
	public static TextSpan FromLength(int start, int length) =>
		new(start, start + length);
	/// <summary>
	/// Creates an empty <see cref="TextSpan"/> with a position.
	/// </summary>
	/// <param name="position">The position of the span.</param>
	/// <returns>A new <see cref="TextSpan"/> with a
	/// start and end position of <paramref name="position"/>.</returns>
	public static TextSpan FromEmpty(int position) =>
		new(position, position);

	public bool Equals(TextSpan other) =>
		Start == other.Start && End == other.End;
	public override bool Equals(object? obj) =>
		obj is TextSpan textSpan &&
		Equals(textSpan);
	public override int GetHashCode() =>
		HashCode.Combine(Start, End);
	public override string ToString() =>
		$"{Start}..{End}";



	public static bool operator ==(TextSpan a, TextSpan b) =>
		a.Equals(b);
	public static bool operator !=(TextSpan a, TextSpan b) =>
		!a.Equals(b);

}
