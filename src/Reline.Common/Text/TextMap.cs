using System;
using System.Collections.Generic;

namespace Reline.Common.Text;

/// <summary>
/// A map of the lines in a string.
/// </summary>
public sealed class TextMap : IEquatable<TextMap>, IEquatable<string> {

	private readonly TextSpan[] lines;

	/// <summary>
	/// The original string the map is of.
	/// </summary>
	public string Text { get; }



	private TextMap(TextSpan[] lines, string text) {
		this.lines = lines;
		Text = text;
	}



	/// <summary>
	/// Creates a new <see cref="TextMap"/> instance.
	/// </summary>
	/// <param name="text">The text to create the map from.</param>
	public static TextMap Create(string text) =>
		new(GetLines(text), text);
	private static TextSpan[] GetLines(string text) {
		List<TextSpan> lines = new();
		int lastLineStart = 0;
		int lastLength = -1;

		for (int i = 0; i < text.Length + 1; i++) {
			lastLength++;
			if (i > 0 && text[i - 1] == '\n') {
				lines.Add(TextSpan.FromLength(lastLineStart, lastLength));
				lastLineStart = i;
				lastLength = 0;
			}
		}
		lines.Add(TextSpan.FromLength(lastLineStart, lastLength));

		return lines.ToArray();
	}

	/// <summary>
	/// Gets the line with a specified line number.
	/// </summary>
	/// <param name="line">The 0-indexed line number of the line to get.</param>
	/// <returns>A <see cref="TextLine"/> representing
	/// the line with the specified line number.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="line"/> was less than 0 or
	/// greater than the amount of lines.
	/// </exception>
	public TextLine GetLine(int line) {
		if (line < 0 || line >= lines.Length)
			throw new ArgumentOutOfRangeException(nameof(line));

		var textSpan = lines[line];
		var text = Text.AsSpan().Slice(textSpan);
		return new(line, textSpan, text);
	}
	/// <summary>
	/// Gets the line at a specified position in the text.
	/// </summary>
	/// <param name="position">The position to get the line of.</param>
	/// <returns>A <see cref="TextLine"/> representing
	/// the line at <paramref name="position"/> in the text.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="position"/> is less than 0 or
	/// greater than the length of the text.
	/// </exception>
	public TextLine GetLineAt(int position) {
		if (position < 0 || position >= Text.Length)
			throw new ArgumentOutOfRangeException(nameof(position));

		for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++) {
			var line = lines[lineNumber];
			if (line.Contains(position)) {
				var text = Text.AsSpan().Slice(line);
				return new(lineNumber, line, text);
			}
		}

		// This should never happen
		throw new InvalidOperationException();
	}
	/// <summary>
	/// Gets the lines within a specified span of the text.
	/// </summary>
	/// <param name="textSpan">The span to get the lines of in the text.</param>
	/// <returns>A new <see cref="TextLinesEnumerator"/> used to
	/// enumerate the lines within <paramref name="textSpan"/>.</returns>
	public TextLinesEnumerator GetLineAt(TextSpan textSpan) =>
		new(Text, textSpan, lines);

	public bool Equals(TextMap? other) =>
		other is not null &&
		Text == other.Text;
	public bool Equals(string? other) =>
		Text == other;
	public override bool Equals(object? obj) =>
		obj is TextMap textMap &&
		Equals(textMap);
	public override int GetHashCode() =>
		Text.GetHashCode();
	public override string ToString() =>
		$"{{{lines.Length} lines}} {Text}";



	/// <summary>
	/// Represents a line of text.
	/// </summary>
	public readonly ref struct TextLine {

		/// <summary>
		/// The line number of the line.
		/// </summary>
		public int LineNumber { get; }
		/// <summary>
		/// The <see cref="Text.TextSpan"/> representation of the line.
		/// </summary>
		public TextSpan TextSpan { get; }
		/// <summary>
		/// The text of the line.
		/// </summary>
		public ReadOnlySpan<char> Text { get; }

		internal TextLine(int lineNumber, TextSpan textSpan, ReadOnlySpan<char> text) {
			LineNumber = lineNumber;
			TextSpan = textSpan;
			Text = text;
		}

		public void Deconstruct(out int lineNumber, out TextSpan textSpan, out ReadOnlySpan<char> text) {
			lineNumber = LineNumber;
			textSpan = TextSpan;
			text = Text;
		}

	}

	/// <summary>
	/// Enumerates the lines within a <see cref="TextSpan"/>.
	/// </summary>
	/// <remarks>
	/// This struct can be used both as an enumerable and enumerator,
	/// though due to being a <see langword="ref"/> struct it does not implement
	/// <see cref="IEnumerable{T}"/> nor <see cref="IEnumerator{T}"/>.
	/// </remarks>
	public ref struct TextLinesEnumerator {

		private readonly ReadOnlySpan<char> text;
		private readonly TextSpan target;
		private readonly TextSpan[] lines;
		private int index;
		private bool active;
		private TextLine current;
		public TextLine Current =>
			active ? current :
			throw new InvalidOperationException();

		internal TextLinesEnumerator(ReadOnlySpan<char> text, TextSpan target, TextSpan[] lines) {
			this.text = text;
			this.target = target;
			this.lines = lines;
			index = 0;
			active = false;
			current = default;
		}

		public bool MoveNext() {
			active = true;

			for (; index < lines.Length; index++) {
				var textSpan = lines[index];
				if (TextSpan.Overlap(target, textSpan)) {
					current = new(index, textSpan, text.Slice(textSpan));
					index++;
					return true;
				}
			}

			active = false;
			return false;
		}
		public TextLinesEnumerator GetEnumerator() =>
			this;

	}

}
