namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a <c>move</c> statement.
/// </summary>
public sealed class MoveStatementSymbol : ManipulationStatementSymbol {

	internal MoveStatementSymbol(IExpressionSymbol source, IExpressionSymbol target)
		: base(source, target) { }
}
