namespace Reline.Compilation.Symbols;

public sealed class UnaryFunctionPointerExpressionSymbol : SymbolNode, IExpressionSymbol {

	// Implement better functions later
	public string Identifier { get; set; } = null!;
	public bool IsConstant => true;

}
