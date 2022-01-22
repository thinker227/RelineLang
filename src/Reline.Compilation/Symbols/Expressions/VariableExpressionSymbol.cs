namespace Reline.Compilation.Symbols;

public sealed class VariableExpressionSymbol : SymbolNode, IExpressionSymbol {

	public VariableSymbol Variable { get; set; } = null!;
	public bool IsConstant => false;

}
