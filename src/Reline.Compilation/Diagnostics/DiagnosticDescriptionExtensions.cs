namespace Reline.Compilation.Diagnostics;

public static class DiagnosticDescriptionExtensions {

	/// <summary>
	/// Formats a <see cref="DiagnosticDescription"/> into a <see cref="Diagnostic"/>.
	/// </summary>
	/// <param name="description">The <see cref="DiagnosticDescription"/> to format.</param>
	/// <param name="location">The span in the source text of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	/// <returns>A new <see cref="Diagnostic"/> created from <paramref name="description"/>.</returns>
	public static Diagnostic ToDiagnostic(this DiagnosticDescription description, TextSpan? location, params object?[] formatArgs) =>
		Diagnostic.Create(description, location, formatArgs);
	/// <summary>
	/// Formats a <see cref="DiagnosticDescription"/> into a <see cref="Diagnostic"/>.
	/// </summary>
	/// <param name="description">The <see cref="DiagnosticDescription"/> to format.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	/// <returns>A new <see cref="Diagnostic"/> created from <paramref name="description"/>.</returns>
	public static Diagnostic ToDiagnostic(this DiagnosticDescription description, params object?[] formatArgs) =>
		Diagnostic.Create(description, null, formatArgs);

}
