namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a <c>swap</c> statement.
/// </summary>
public sealed class SwapStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The source range.
	/// </summary>
	public IExpressionSymbol Source { get; set; } = null!;
	/// <summary>
	/// The target range.
	/// </summary>
	public IExpressionSymbol Target { get; set; } = null!;

}
