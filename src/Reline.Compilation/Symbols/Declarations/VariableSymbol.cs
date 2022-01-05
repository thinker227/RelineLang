namespace Reline.Compilation.Symbols;

public sealed class VariableSymbol : SymbolNode {

	public string Identifier { get; set; } = null!;
	public ITypeSymbol Type { get; set; } = null!;

}
