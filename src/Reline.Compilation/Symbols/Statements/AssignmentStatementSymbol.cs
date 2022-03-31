namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an assignment statement.
/// </summary>
public sealed class AssignmentStatementSymbol : SymbolNode, IStatementSymbol {
	
	/// <summary>
	/// The variable being assigned.
	/// </summary>
	public IVariableSymbol? Variable { get; }
	/// <summary>
	/// The initializer expression.
	/// </summary>
	public IExpressionSymbol? Initializer { get; }



	internal AssignmentStatementSymbol(IVariableSymbol? variable, IExpressionSymbol? initializer) {
		Variable = variable;
		Initializer = initializer;
	}



	public override IEnumerable<ISymbol> GetChildren() {
		if (Initializer is not null) yield return Initializer;
	}

}
