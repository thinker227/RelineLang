using System.Collections;

namespace Reline.Compilation.Symbols;

/// <summary>
/// A dynamically expanding 1-indexed list of lines.
/// </summary>
public sealed class LineList : IReadOnlyList<LineSymbol> {

	private LineSymbol[] lines;



	public int Count => lines.Length;
	// LineList is 1-indexed, because lines are 1-indexed
	/// <summary>
	/// Gets or sets an item in the list. If the index an element is assigned to
	/// is greater than the size of the list, the list will expand to fit the element.
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// The get/set index is less than 0 or the get index is greater than the size of the list.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// An existing item is attempted to be overwritten.
	/// </exception>
	public LineSymbol this[int index] {
		get {
			int actualIndex = index - 1;
			return actualIndex < 0 || actualIndex >= lines.Length ?
				throw new ArgumentOutOfRangeException(nameof(index)) :
				lines[actualIndex];
		}
		set {
			int actualIndex = index - 1;
			if (value is null) throw new ArgumentNullException(nameof(value));
			if (actualIndex < 0) throw new ArgumentOutOfRangeException(nameof(index));
			if (actualIndex >= lines.Length) GrowArray(index);
			if (lines[actualIndex] is not null) throw new InvalidOperationException("Cannot override lines.");

			lines[actualIndex] = value;
		}
	}



	public LineList() {
		lines = Array.Empty<LineSymbol>();
	}



	private void GrowArray(int newSize) {
		var oldLines = lines;
		lines = new LineSymbol[newSize];
		oldLines.CopyTo(lines, 0);
	}

	public IEnumerator<LineSymbol> GetEnumerator() {
		foreach (var line in lines)
			yield return line;
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
