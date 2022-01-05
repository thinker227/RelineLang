namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an expression.
/// </summary>
public interface IExpressionSymbol : ISymbol {

	/// <summary>
	/// The type of the expression.
	/// </summary>
	ITypeSymbol Type { get; }
	/// <summary>
	/// Whether the expression is a constant and can be evaluated at compile-time.
	/// </summary>
	bool IsConstant { get; }

}
