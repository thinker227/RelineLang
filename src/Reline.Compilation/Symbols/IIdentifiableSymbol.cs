namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a symbol which can be identified through an identifier.
/// </summary>
public interface IIdentifiableSymbol : ISymbol {

	string Identifier { get; }

}
