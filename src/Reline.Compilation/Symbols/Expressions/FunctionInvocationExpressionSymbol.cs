namespace Reline.Compilation.Symbols;

public sealed class FunctionInvocationExpressionSymbol : SymbolNode, IExpressionSymbol {

	public string Identifier { get; set; } = null!;
	public ICollection<IExpressionSymbol> Arguments { get; } = new List<IExpressionSymbol>();
	public bool IsConstant => false;

}
