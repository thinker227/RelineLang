namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a <c>copy</c> statement.
/// </summary>
public sealed class CopyStatementSymbol : ManipulationStatementSymbol {

	internal CopyStatementSymbol(IExpressionSymbol source, IExpressionSymbol target)
		: base(source, target) { }

}
