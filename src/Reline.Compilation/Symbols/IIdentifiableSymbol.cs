namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a symbol which can be identified through an identifier.
/// </summary>
public interface IIdentifiableSymbol : ISymbol {

	/// <summary>
	/// The identifier of the symbol.
	/// </summary>
	string Identifier { get; }

}
