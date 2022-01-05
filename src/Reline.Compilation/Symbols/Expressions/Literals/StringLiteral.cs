namespace Reline.Compilation.Symbols;

public sealed class StringLiteral : SymbolNode, ILiteralSymbol {

	public string Value { get; set; } = null!;
	object ILiteralSymbol.Value => Value;
	public ITypeSymbol Type => StringType.Instance;

}
