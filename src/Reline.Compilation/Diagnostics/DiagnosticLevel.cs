namespace Reline.Compilation.Diagnostics;

/// <summary>
/// The level of a <see cref="Diagnostic"/>.
/// </summary>
public enum DiagnosticLevel {
	/// <summary>
	/// The diagnostic is not shown.
	/// </summary>
	Hidden,
	/// <summary>
	/// The diagnostic is information.
	/// </summary>
	Info,
	/// <summary>
	/// The diagnostic is a warning.
	/// </summary>
	Warning,
	/// <summary>
	/// The diagnostic is a fatal error.
	/// </summary>
	Error
}

public static class DiagnosticLevelExtensions {

	/// <summary>
	/// Returns the capitalized display string of a <see cref="DiagnosticLevel"/>.
	/// </summary>
	/// <param name="diagnosticLevel">The level to get the capitalized display string of.</param>
	/// <returns>The capitalized display string of <paramref name="diagnosticLevel"/>.</returns>
	public static string ToDisplayString(this DiagnosticLevel diagnosticLevel) => diagnosticLevel switch {
		DiagnosticLevel.Hidden => "Hidden",
		DiagnosticLevel.Info => "Info",
		DiagnosticLevel.Warning => "Warning",
		DiagnosticLevel.Error => "Error",
		_ => throw new InvalidOperationException()
	};
	/// <summary>
	/// Returns the lowercase display string of a <see cref="DiagnosticLevel"/>.
	/// </summary>
	/// <param name="diagnosticLevel">The level to get the lowercase display string of.</param>
	/// <returns>The lowercase display string of <paramref name="diagnosticLevel"/>.</returns>
	public static string ToLowercaseDisplayString(this DiagnosticLevel diagnosticLevel) => diagnosticLevel switch {
		DiagnosticLevel.Hidden => "hidden",
		DiagnosticLevel.Info => "info",
		DiagnosticLevel.Warning => "warning",
		DiagnosticLevel.Error => "error",
		_ => throw new InvalidOperationException()
	};

}
