namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function declaration statement.
/// </summary>
public sealed class FunctionDeclarationStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The function declared by the declaration.
	/// May be <see langword="null"/> if the function is invalid.
	/// </summary>
	public FunctionSymbol? Function { get; set; }
	/// <summary>
	/// The <see cref="IExpressionSymbol"/> describing the range of the function.
	/// </summary>
	public IExpressionSymbol RangeExpression { get; set; } = null!;



	public override IEnumerable<ISymbol> GetChildren() {
		// This is the only way to get the range expression as a child
		// because the function is never returned as a child.
		yield return RangeExpression;
	}

}
