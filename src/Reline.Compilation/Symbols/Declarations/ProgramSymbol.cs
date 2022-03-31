namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a program.
/// </summary>
public sealed class ProgramSymbol : SymbolNode {

	/// <summary>
	/// The lines of the program.
	/// </summary>
	public LineSymbol[] Lines { get; }
	/// <summary>
	/// The line number of first line of the program.
	/// </summary>
	public int StartLine { get; }
	/// <summary>
	/// The line number of the last line of the program.
	/// </summary>
	public int EndLine { get; }
	/// <summary>
	/// The full range of the program.
	/// </summary>
	public RangeValue FullRange => new(StartLine, EndLine);



	internal ProgramSymbol(int startLine, int endLine) {
		StartLine = startLine;
		EndLine = endLine;
		Lines = new LineSymbol[FullRange.Length];
	}



	// Labels, variables and functions are references and not children
	public override IEnumerable<ISymbol> GetChildren() =>
		Lines; 

}
