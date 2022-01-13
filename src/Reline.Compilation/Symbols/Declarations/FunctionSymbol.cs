namespace Reline.Compilation.Symbols.Declarations;

public sealed class FunctionSymbol : SymbolNode {

	public string Identifier { get; set; } = null!;
	public IExpressionSymbol BodyExpression { get; set; } = null!;
	public ICollection<LineSymbol> Body { get; } = new List<LineSymbol>();
	public ITypeSymbol ReturnType { get; set; } = null!;
	public ICollection<VariableSymbol> Parameters { get; } = new List<VariableSymbol>();

}
