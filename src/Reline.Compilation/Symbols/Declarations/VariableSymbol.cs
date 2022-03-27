namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents either a variable or parameter.
/// </summary>
public interface IVariableSymbol : IDefinedIdentifiableSymbol, IEquatable<IVariableSymbol> {

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
	ICollection<ISymbol> IDefinedIdentifiableSymbol.References => References;



	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

	public bool Equals(IVariableSymbol? other) =>
		other is VariableSymbol &&
		Identifier == other.Identifier;
	public override bool Equals(object? obj) =>
		obj is VariableSymbol variable &&
		Equals(variable);
	public override int GetHashCode() =>
		Identifier.GetHashCode();

	public override string ToString() =>
		Identifier;

}

/// <summary>
/// Represents a parameter.
/// </summary>
public sealed class ParameterSymbol : SymbolNode, IVariableSymbol, IScopedIdentifiableSymbol {

	/// <summary>
	/// The parameter identifier.
	/// </summary>
	public string Identifier { get; set; } = null!;
	public IList<ISymbol> References { get; } = new List<ISymbol>();
	ICollection<ISymbol> IDefinedIdentifiableSymbol.References => References;
	/// <summary>
	/// The range the parameter is valid within.
	/// </summary>
	public RangeValue Range { get; set; }
	RangeValue IScopedIdentifiableSymbol.Scope => Range;
	/// <summary>
	/// The function the parameter is a parameter to.
	/// May be <see langword="null"/> if the function is invalid.
	/// </summary>
	public FunctionSymbol? Function { get; set; }



	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

	public bool Equals(IVariableSymbol? other) =>
		other is ParameterSymbol parameter &&
		Identifier == parameter.Identifier &&
		Range == parameter.Range;
	public override bool Equals(object? obj) =>
		obj is ParameterSymbol variable &&
		Equals(variable);
	public override int GetHashCode() =>
		HashCode.Combine(Identifier, Range);

	public override string ToString() =>
		Identifier;

}
