namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a manipulation statement.
/// </summary>
public interface IManipulationStatementSymbol : IStatementSymbol {

	/// <summary>
	/// The <see cref="IExpressionSymbol"/> indicating the source range.
	/// </summary>
	IExpressionSymbol Source { get; }
	/// <summary>
	/// The <see cref="IExpressionSymbol"/> indicating the target line or range.
	/// </summary>
	IExpressionSymbol Target { get; }

}

/// <summary>
/// A manipulation statement.
/// </summary>
public abstract class ManipulationStatementSymbol : SymbolNode, IManipulationStatementSymbol {

	public IExpressionSymbol Source { get; }
	public IExpressionSymbol Target { get; }



	internal ManipulationStatementSymbol(IExpressionSymbol source, IExpressionSymbol target) {
		Source = source;
		Target = target;
	}



	public override IEnumerable<ISymbol> GetChildren() {
		yield return Source;
		yield return Target;
	}

}
