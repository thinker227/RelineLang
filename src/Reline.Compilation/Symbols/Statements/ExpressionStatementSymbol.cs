namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an expression statement.
/// </summary>
public sealed class ExpressionStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The expression of the statement.
	/// </summary>
	public IExpressionSymbol Expression { get; set; } = null!;

}
