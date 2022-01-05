namespace Reline.Compilation.Symbols;

public sealed class AssignmentStatementSymbol : SymbolNode, IStatementSymbol {

	public VariableSymbol Variable { get; set; } = null!;
	public IExpressionSymbol Initializer { get; set; } = null!;

}
