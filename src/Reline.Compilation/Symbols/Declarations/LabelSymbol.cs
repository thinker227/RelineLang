namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a label.
/// </summary>
public sealed class LabelSymbol : SymbolNode, IDefinedIdentifiableSymbol {

	/// <summary>
	/// The label identifier.
	/// </summary>
	public string Identifier { get; }
	/// <summary>
	/// The line of the label.
	/// </summary>
	public LineSymbol Line { get; }
	/// <summary>
	/// The references to the label.
	/// </summary>
	public IList<ISymbol> References { get; } = new List<ISymbol>();
	ICollection<ISymbol> IDefinedIdentifiableSymbol.References => References;



	internal LabelSymbol(string identifier, LineSymbol line) {
		Identifier = identifier;
		Line = line;
	}



	public override IEnumerable<ISymbol> GetChildren() {
		yield return Line;
	}

}
