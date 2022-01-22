namespace Reline.Compilation.Symbols;

public sealed class VariableSymbol : SymbolNode, IIdentifiableSymbol, IEquatable<VariableSymbol> {

	public string Identifier { get; set; } = null!;



	public bool Equals(VariableSymbol? other) =>
		other is not null &&
		Identifier == other.Identifier;
	public override bool Equals(object? obj) =>
		obj is VariableSymbol variable &&
		Equals(variable);
	public override int GetHashCode() =>
		Identifier.GetHashCode();

}
