namespace Reline.Compilation.Symbols;

/// <summary>
/// Exception thrown when the type of an <see cref="IExpressionSymbol"/> is inconclusive.
/// </summary>
public class InconclusiveTypeException : Exception {

	/// <summary>
	/// Initializes a new <see cref="InconclusiveTypeException"/> instance.
	/// </summary>
	public InconclusiveTypeException() { }
	/// <summary>
	/// Initializes a new <see cref="InconclusiveTypeException"/> instance.
	/// </summary>
	/// <param name="message">The message describing the exception.</param>
	public InconclusiveTypeException(string? message) : base(message) { }
	/// <summary>
	/// Initializes a new <see cref="InconclusiveTypeException"/> instance.
	/// </summary>
	/// <param name="message">The message describing the exception.</param>
	/// <param name="innerException">The inner exception which caused the current exception.</param>
	public InconclusiveTypeException(string? message, Exception? innerException) : base(message, innerException) { }

}