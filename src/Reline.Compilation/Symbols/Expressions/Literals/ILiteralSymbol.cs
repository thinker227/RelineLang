namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents literal values.
/// </summary>
public interface ILiteralSymbol : ISymbol {

	/// <summary>
	/// The type of the literal.
	/// </summary>
	ITypeSymbol Type { get; }
	/// <summary>
	/// The value of the literal.
	/// </summary>
	object Value { get; }

}
