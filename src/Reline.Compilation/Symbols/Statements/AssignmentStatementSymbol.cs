namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an assignment statement.
/// </summary>
public sealed class AssignmentStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The variable being assigned.
	/// </summary>
	public IVariableSymbol? Variable { get; set; } = null!;
	/// <summary>
	/// The initializer expression.
	/// </summary>
	public IExpressionSymbol Initializer { get; set; } = null!;

}
