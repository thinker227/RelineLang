namespace Reline.Common;

/// <summary>
/// Represents an entity visitable by a visitor.
/// </summary>
public interface IVisitable {

	/// <summary>
	/// Accepts an <see cref="IVisitor"/> returning nothing.
	/// </summary>
	/// <param name="visitor">The visitor to accept.</param>
	void Accept(IVisitor visitor);
	/// <summary>
	/// Accepts an <see cref="IVisitor"/> returning a generic result.
	/// </summary>
	/// <typeparam name="T">The return type of the visitor.</typeparam>
	/// <param name="visitor">The visitor to accept.</param>
	T Accept<T>(IVisitor<T> visitor);

}
