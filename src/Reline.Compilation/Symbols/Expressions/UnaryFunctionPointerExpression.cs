namespace Reline.Compilation.Symbols;

public sealed class UnaryFunctionPointerExpression : SymbolNode, IExpressionSymbol {

	// Implement better functions later
	public string Identifier { get; set; } = null!;
	public bool IsConstant => true;

}
