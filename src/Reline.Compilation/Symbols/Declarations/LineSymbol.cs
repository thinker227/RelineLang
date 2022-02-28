namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a line.
/// </summary>
public sealed class LineSymbol : SymbolNode {

	/// <summary>
	/// The label of the line.
	/// </summary>
	public LabelSymbol? Label { get; set; }
	/// <summary>
	/// The statement of the line.
	/// </summary>
	public IStatementSymbol? Statement { get; set; }
	/// <summary>
	/// The line's number.
	/// </summary>
	public int LineNumber { get; set; }



	public override IEnumerable<ISymbol> GetChildren() {
		if (Label is not null) yield return Label;
		if (Statement is not null) yield return Statement;
	}

}
