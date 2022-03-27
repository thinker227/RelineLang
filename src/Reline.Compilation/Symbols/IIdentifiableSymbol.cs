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

/// <summary>
/// Represents an identifiable symbol which is defined in source code.
/// </summary>
public interface IDefinedIdentifiableSymbol : IIdentifiableSymbol {

	/// <summary>
	/// The references to the symbol.
	/// </summary>
	ICollection<ISymbol> References { get; }

}

/// <summary>
/// Represents an identifiable symbol which belongs to a scope.
/// </summary>
public interface IScopedIdentifiableSymbol : IDefinedIdentifiableSymbol {

	/// <summary>
	/// The scope the identifier exists within.
	/// </summary>
	RangeValue Scope { get; }

}
