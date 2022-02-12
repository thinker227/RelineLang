namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an expression.
/// </summary>
public interface IExpressionSymbol : ISymbol {

	/// <summary>
	/// Accepts an <see cref="IExpressionVisitor{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the visitor.</typeparam>
	/// <param name="visitor">The <see cref="IExpressionVisitor{T}"/> to accept.</param>
	/// <returns>The return value of <paramref name="visitor"/>.</returns>
	T Accept<T>(IExpressionVisitor<T> visitor);

}
