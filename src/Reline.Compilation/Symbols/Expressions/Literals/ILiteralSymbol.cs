namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents literal values.
/// </summary>
public interface ILiteralSymbol : ISymbol {

	/// <summary>
	/// The value of the literal.
	/// </summary>
	object Value { get; }

}
