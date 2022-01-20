namespace Reline.Compilation.Symbols;

public sealed class NumberLiteral : SymbolNode, ILiteralSymbol {

	public int Value { get; set; }
	object ILiteralSymbol.Value => Value;

}
