namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a <c>copy</c> statement.
/// </summary>
public sealed class CopyStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The source range.
	/// </summary>
	public IExpressionSymbol Source { get; set; } = null!;
	/// <summary>
	/// The target line.
	/// </summary>
	public IExpressionSymbol Target { get; set; } = null!;

}
