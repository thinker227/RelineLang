namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a type.
/// </summary>
public interface ITypeSymbol : ISymbol, IEquatable<ITypeSymbol> {

	/// <summary>
	/// Whether the type is native.
	/// </summary>
	bool IsNative { get; }
	/// <summary>
	/// The name of the type.
	/// </summary>
	string TypeName { get; }

}
