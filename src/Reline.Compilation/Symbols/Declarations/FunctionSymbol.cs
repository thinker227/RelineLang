namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function.
/// </summary>
public sealed class FunctionSymbol : SymbolNode, IIdentifiableSymbol {

	/// <summary>
	/// The function identifier.
	/// </summary>
	public string Identifier { get; set; } = null!;
	/// <summary>
	/// The <see cref="FunctionDeclarationStatementSymbol"/> which declared this function.
	/// </summary>
	public FunctionDeclarationStatementSymbol Declaration { get; set; } = null!;
	/// <summary>
	/// The <see cref="IExpressionSymbol"/> describing the range of the function.
	/// </summary>
	public IExpressionSymbol RangeExpression { get; set; } = null!;
	/// <summary>
	/// The range of the function.
	/// </summary>
	public RangeLiteral Range { get; set; }
	/// <summary>
	/// The lines of the body of the function.
	/// </summary>
	public ICollection<LineSymbol> Body { get; } = new List<LineSymbol>();
	/// <summary>
	/// The parameters of the function.
	/// </summary>
	public ICollection<ParameterSymbol> Parameters { get; } = new List<ParameterSymbol>();
	/// <summary>
	/// The references to the function.
	/// </summary>
	public IList<ISymbol> References { get; } = new List<ISymbol>();



	public override IEnumerable<ISymbol> GetChildren() {
		// The declaration and references are
		// more or less parents rather than children
		yield return RangeExpression;
		foreach (var line in Body) yield return line;
		foreach (var param in Parameters) yield return param;
	}

}
