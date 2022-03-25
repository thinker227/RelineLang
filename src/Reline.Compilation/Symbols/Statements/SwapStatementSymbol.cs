namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a <c>swap</c> statement.
/// </summary>
public sealed class SwapStatementSymbol : ManipulationStatementSymbol {

	public SwapStatementSymbol(IExpressionSymbol source, IExpressionSymbol target)
		: base(source, target) { }

}
