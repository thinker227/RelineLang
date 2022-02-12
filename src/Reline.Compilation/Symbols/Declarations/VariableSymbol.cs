namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents either a variable or parameter.
/// </summary>
public interface IVariableSymbol : IIdentifiableSymbol, IEquatable<IVariableSymbol> {

	/// <summary>
	/// The references to the variable.
	/// </summary>
	IList<ISymbol> References { get; }

}

/// <summary>
/// Represents a variable.
/// </summary>
public sealed class VariableSymbol : SymbolNode, IVariableSymbol {

	/// <summary>
	/// The variable identifier.
	/// </summary>
	public string Identifier { get; set; } = null!;
	public IList<ISymbol> References { get; } = new List<ISymbol>();



	public bool Equals(IVariableSymbol? other) =>
		other is VariableSymbol &&
		Identifier == other.Identifier;
	public override bool Equals(object? obj) =>
		obj is VariableSymbol variable &&
		Equals(variable);
	public override int GetHashCode() =>
		Identifier.GetHashCode();

}

/// <summary>
/// Represents a parameter.
/// </summary>
public sealed class ParameterSymbol : SymbolNode, IVariableSymbol {

	/// <summary>
	/// The parameter identifier.
	/// </summary>
	public string Identifier { get; set; } = null!;
	public IList<ISymbol> References { get; } = new List<ISymbol>();
	/// <summary>
	/// The range the parameter is valid within.
	/// </summary>
	public RangeLiteral Range { get; set; }
	/// <summary>
	/// The function the parameter is a parameter to.
	/// </summary>
	public FunctionSymbol Function { get; set; } = null!;



	public bool Equals(IVariableSymbol? other) =>
		other is ParameterSymbol parameter &&
		Identifier == parameter.Identifier &&
		Range == parameter.Range;
	public override bool Equals(object? obj) =>
		obj is ParameterSymbol variable &&
		Equals(variable);
	public override int GetHashCode() =>
		HashCode.Combine(Identifier, Range);
}
