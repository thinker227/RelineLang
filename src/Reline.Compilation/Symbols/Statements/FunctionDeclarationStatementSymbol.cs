namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function declaration statement.
/// </summary>
public sealed class FunctionDeclarationStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The function declared by the declaration.
	/// </summary>
	public FunctionSymbol Function { get; set; } = null!;



	public override IEnumerable<ISymbol> GetChildren() {
		yield return Function;
	}

}
