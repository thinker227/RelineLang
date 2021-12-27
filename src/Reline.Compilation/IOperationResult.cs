using Reline.Compilation.Diagnostics;

namespace Reline.Compilation;

/// <summary>
/// Represents the result of a compilation operation.
/// </summary>
/// <typeparam name="T">The type of the result of the operation.</typeparam>
public interface IOperationResult<T> {

	/// <summary>
	/// The diagnostics produced by the operation.
	/// </summary>
	ImmutableArray<Diagnostic> Diagnostics { get; }
	/// <summary>
	/// The result of the operation.
	/// </summary>
	T Result { get; }

}
