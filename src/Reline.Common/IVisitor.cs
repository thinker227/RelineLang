namespace Reline.Common;

/// <summary>
/// Represents a visitor.
/// </summary>
public interface IVisitor {

	/// <summary>
	/// Visits a visitable entity.
	/// </summary>
	/// <param name="visitable">The <see cref="IVisitable"/> to visit.</param>
	void Visit(IVisitable visitable);

}

/// <summary>
/// Represents a generic visitor.
/// </summary>
/// <typeparam name="T">The return type of the visitor.</typeparam>
public interface IVisitor<out T> {

	/// <summary>
	/// Visits a visitable entity.
	/// </summary>
	/// <param name="visitable">The <see cref="IVisitable"/> to visit.</param>
	/// <returns>The return value of the visitor.</returns>
	T Visit(IVisitable visitable);

}
