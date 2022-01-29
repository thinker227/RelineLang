namespace Reline.Compilation.Symbols;

public sealed class AssignmentStatementSymbol : SymbolNode, IStatementSymbol {

	public IVariableSymbol Variable { get; set; } = null!;
	public IExpressionSymbol Initializer { get; set; } = null!;

}
