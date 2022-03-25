namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a <c>return</c> statement.
/// </summary>
public sealed class ReturnStatementSymbol : SymbolNode, IStatementSymbol {

	/// <summary>
	/// The expression being returned.
	/// </summary>
	public IExpressionSymbol Expression { get; }
	/// <summary>
	/// The function being returned from.
	/// </summary>
	public FunctionSymbol? Function { get; }



	internal ReturnStatementSymbol(IExpressionSymbol expression, FunctionSymbol? function) {
		Expression = expression;
		Function = function;
	}



	public override IEnumerable<ISymbol> GetChildren() {
		yield return Expression;
	}

}
