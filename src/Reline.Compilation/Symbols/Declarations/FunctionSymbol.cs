namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a function.
/// </summary>
public interface IFunctionSymbol : IIdentifiableSymbol {

	/// <summary>
	/// The arity of the function.
	/// </summary>
	int Arity { get; }

}

/// <summary>
/// Represents a user-defined function.
/// </summary>
public sealed class FunctionSymbol : SymbolNode, IFunctionSymbol, IDefinedIdentifiableSymbol {

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
	public IExpressionSymbol RangeExpression => Declaration.RangeExpression;
	/// <summary>
	/// The range of the function.
	/// </summary>
	public RangeValue Range { get; set; }
	/// <summary>
	/// The lines of the body of the function.
	/// </summary>
	public ICollection<LineSymbol> Body { get; } = new List<LineSymbol>();
	/// <summary>
	/// The parameters of the function.
	/// </summary>
	public ICollection<ParameterSymbol> Parameters { get; } = new List<ParameterSymbol>();
	public int Arity => Parameters.Count;
	/// <summary>
	/// The references to the function.
	/// </summary>
	public IList<ISymbol> References { get; } = new List<ISymbol>();
	ICollection<ISymbol> IDefinedIdentifiableSymbol.References => References;



	public override IEnumerable<ISymbol> GetChildren() {
		// The declaration and references are
		// more or less parents rather than children
		yield return RangeExpression;
		foreach (var line in Body) yield return line;
		foreach (var param in Parameters) yield return param;
	}

}

/// <summary>
/// Represents a native function.
/// </summary>
public sealed class NativeFunctionSymbol : SymbolNode, IFunctionSymbol {

	public int Arity { get; }
	/// <summary>
	/// The function identifier.
	/// </summary>
	public string Identifier { get; }
	/// <summary>
	/// The type the native function represents.
	/// </summary>
	public NativeFunction FunctionType { get; }



	public NativeFunctionSymbol(string identifier, int arity, NativeFunction functionType) {
		Identifier = identifier;
		Arity = arity;
		FunctionType = functionType;
	}



	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

}
