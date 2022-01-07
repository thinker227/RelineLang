namespace Reline.Common;

/// <summary>
/// Represents a visitor.
/// </summary>
/// <typeparam name="TObject">The type of object the visitor visits.</typeparam>
public interface IVisitor<in TObject> where TObject : IVisitable<TObject> {

	/// <summary>
	/// Visits a visitable entity.
	/// </summary>
	/// <param name="visitable">The <typeparamref name="TObject"/> to visit.</param>
	void Visit(TObject visitable);

}
/// <summary>
/// Represents a visitor with a return type.
/// </summary>
/// <typeparam name="TObject">The type of object the visitor visits.</typeparam>
/// <typeparam name="TResult">The result of a visit..</typeparam>
public interface IVisitor<in TObject, out TResult> where TObject : IVisitable<TObject> {

	/// <summary>
	/// Visits a visitable entity.
	/// </summary>
	/// <param name="visitable">The <typeparamref name="TObject"/> to visit.</param>
	/// <returns>The result of the visit.</returns>
	TResult Visit(TObject visitable);

}
