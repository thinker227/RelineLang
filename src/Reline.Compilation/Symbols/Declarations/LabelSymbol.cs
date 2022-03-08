namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a label.
/// </summary>
public sealed class LabelSymbol : SymbolNode, IDefinedIdentifiableSymbol {

	/// <summary>
	/// The label identifier.
	/// </summary>
	public string Identifier { get; set; } = null!;
	/// <summary>
	/// The line of the label.
	/// </summary>
	public LineSymbol Line { get; set; } = null!;
	/// <summary>
	/// The references to the label.
	/// </summary>
	public IList<ISymbol> References { get; } = new List<ISymbol>();
	ICollection<ISymbol> IDefinedIdentifiableSymbol.References => References;



	public override IEnumerable<ISymbol> GetChildren() {
		yield return Line;
	}

}
