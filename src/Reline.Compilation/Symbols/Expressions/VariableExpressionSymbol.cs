namespace Reline.Compilation.Symbols;

public sealed class VariableExpressionSymbol : SymbolNode, IExpressionSymbol {

	public VariableSymbol Variable { get; set; } = null!;
	public ITypeSymbol Type => Variable.Type;
	public bool IsConstant => false;

}
