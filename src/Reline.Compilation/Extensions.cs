using Reline.Compilation.Diagnostics;

namespace Reline.Compilation;

public static class Extensions {

	/// <summary>
	/// Returns whether a <see cref="IOperationResult{T}"/> has any fatal errors.
	/// </summary>
	/// <typeparam name="T">The type of the result of the
	/// <see cref="IOperationResult{T}"/>.</typeparam>
	/// <param name="operationResult">The source operation result.</param>
	/// <returns>Whether <paramref name="operationResult"/> contains any diagnostics with
	/// a severity level of <see cref="DiagnosticLevel.Error"/>.</returns>
	public static bool HasErrors<T>(this IOperationResult<T> operationResult) =>
		operationResult.Diagnostics.Any(d => d.Level == DiagnosticLevel.Error);

}
