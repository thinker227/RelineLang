namespace Reline.Compilation;

/// <summary>
/// An exception caused by compilation.
/// </summary>
public class CompilationException : Exception {

	/// <summary>
	/// Initializes a new <see cref="CompilationException"/> instance.
	/// </summary>
	public CompilationException() { }
	/// <summary>
	/// Initializes a new <see cref="CompilationException"/> instance.
	/// </summary>
	/// <param name="message">The message describing the exception.</param>
	public CompilationException(string? message) : base(message) { }
	/// <summary>
	/// Initializes a new <see cref="CompilationException"/> instance.
	/// </summary>
	/// <param name="message">The message describing the exception.</param>
	/// <param name="inner">The inner exception which caused the current exception.</param>
	public CompilationException(string? message, Exception? inner) : base(message, inner) { }

}
