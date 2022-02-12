namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a general variable.
/// </summary>
public interface IVariableSymbol : IIdentifiableSymbol, IEquatable<IVariableSymbol> {

	IReadOnlyCollection<ISymbol> References { get; }
}

/// <summary>
/// Represents a variable.
/// </summary>
public sealed class VariableSymbol : SymbolNode, IVariableSymbol {

	/// <summary>
	/// The variable identifier.
	/// </summary>
	public string Identifier { get; set; } = null!;
	public List<ISymbol> References { get; } = new List<ISymbol>();
	IReadOnlyCollection<ISymbol> IVariableSymbol.References => References;



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
	public List<ISymbol> References { get; } = new List<ISymbol>();
	IReadOnlyCollection<ISymbol> IVariableSymbol.References => References;
	/// <summary>
	/// The range of the parameter.
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
