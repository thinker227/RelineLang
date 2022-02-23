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
