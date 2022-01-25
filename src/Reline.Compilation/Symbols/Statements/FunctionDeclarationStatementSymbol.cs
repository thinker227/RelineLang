namespace Reline.Compilation.Symbols;

public sealed class FunctionDeclarationStatementSymbol : SymbolNode, IStatementSymbol {

	public FunctionSymbol Function { get; set; } = null!;

}
