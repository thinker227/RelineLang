namespace Reline.Common;

/// <summary>
/// Represents an entity visitable by a visitor.
/// </summary>
public interface IVisitable<out T> where T : IVisitable<T> {

	/// <summary>
	/// Accepts an <see cref="IVisitor{TObject}"/> returning nothing.
	/// </summary>
	/// <param name="visitor">The visitor to accept.</param>
	void Accept(IVisitor<T> visitor);
	/// <summary>
	/// Accepts an <see cref="IVisitor{TObject, TResult}"/> returning a result.
	/// </summary>
	/// <typeparam name="T">The return type of the visitor.</typeparam>
	/// <param name="visitor">The visitor to accept.</param>
	TResult Accept<TResult>(IVisitor<T, TResult> visitor);

}
