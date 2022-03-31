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
	/// <summary>
	/// Creates a <see cref="TextSpan"/> with a length of 1.
	/// </summary>
	/// <param name="position">The position of the span.</param>
	/// <returns>A new <see cref="TextSpan"/> with a start position of
	/// <paramref name="position"/> and a length of 1.</returns>
	public static TextSpan FromSingle(int position) =>
		new(position, position + 1);
	/// <summary>
	/// Creates a <see cref="TextSpan"/> from the bounds
	/// of two other text spans.
	/// </summary>
	/// <param name="start">The <see cref="TextSpan"/> to use as the start position.</param>
	/// <param name="end">The <see cref="TextSpan"/> to use as the end position.</param>
	/// <returns>A new <see cref="TextSpan"/> wit the bounds of
	/// <paramref name="start"/> and <paramref name="end"/>.</returns>
	public static TextSpan FromBounds(TextSpan start, TextSpan end) =>
		new(start.Start, end.End);

	/// <summary>
	/// Returns whether two <see cref="TextSpan"/> instances overlap.
	/// </summary>
	/// <param name="a">The first span to check.</param>
	/// <param name="b">The second span to check.</param>
	/// <returns>Whether the bounds of <paramref name="a"/> and
	/// <paramref name="b"/> overlap.</returns>
	public static bool Overlap(TextSpan a, TextSpan b) =>
		a.Contains(b.Start) || a.Contains(b.End - 1) ||
		b.Contains(a.Start) || b.Contains(a.End - 1);
	public static TextSpan Union(TextSpan a, TextSpan b) =>
		Overlap(a, b)
		? new(Math.Max(a.Start, b.Start), Math.Min(a.End, b.End))
		: Empty;
	/// <summary>
	/// Returns whether a given position is within the text span.
	/// </summary>
	/// <param name="pos">The position to check.</param>
	/// <returns>Whether <paramref name="pos"/> is
	/// within the bounds of the text span.</returns>
	public bool Contains(int pos) =>
		pos >= Start && pos < End;

	public bool Equals(TextSpan other) =>
		Start == other.Start && End == other.End;
	public override bool Equals(object? obj) =>
		obj is TextSpan textSpan &&
		Equals(textSpan);
	public override int GetHashCode() =>
		HashCode.Combine(Start, End);
	public override string ToString() =>
		$"{Start}..{End}";
	public void Deconstruct(out int start, out int end) {
		start = Start;
		end = End;
	}



	public static bool operator ==(TextSpan a, TextSpan b) =>
		a.Equals(b);
	public static bool operator !=(TextSpan a, TextSpan b) =>
		!a.Equals(b);

}
