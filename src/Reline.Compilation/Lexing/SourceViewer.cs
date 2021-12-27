namespace Reline.Compilation.Lexing;

public sealed class SourceViewer : IViewer<char> {

	private int position;



	public int Position => position;
	public string Source { get; }
	public char Current => GetAt(position);
	public char Next => GetAt(position + 1);
	public bool IsAtEnd => position >= Source.Length;



	public SourceViewer(string source) {
		position = 0;
		Source = source;
	}



	private char GetAt(int position) =>
		position < Source.Length ? Source[position] : '\0';
	public string GetString(int length) {
		int start = position;
		int end = start + length;
		if (start >= Source.Length) start = Source.Length - 1;
		if (end > Source.Length) end = Source.Length - 1;
		return Source[start..end];
	}

	public void MoveNext() =>
		position++;

	public IEnumerator<char> GetEnumerator() =>
		Source.GetEnumerator();
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
		GetEnumerator();

}
