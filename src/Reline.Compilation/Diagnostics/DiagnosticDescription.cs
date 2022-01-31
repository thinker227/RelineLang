using System.Diagnostics;

namespace Reline.Compilation.Diagnostics;

/// <summary>
/// Describes a formattable diagnostic.
/// </summary>
public sealed record class DiagnosticDescription {

	/// <summary>
	/// The error code of the diagnostic, ex. <c>RL0001</c>.
	/// </summary>
	public string ErrorCode { get; init; } = "RL0000";
	/// <summary>
	/// The description of the diagnostic.
	/// </summary>
	public string Description { get; init; } = "No description";
	/// <summary>
	/// The level of the diagnostic.
	/// </summary>
	public DiagnosticLevel Level { get; init; }



	/// <summary>
	/// Formats the description of the diagnostic.
	/// </summary>
	/// <param name="args">The arguments to format the description with.</param>
	/// <returns>The formatted description of the diagnostic.</returns>
	public string FormatDescription(params object?[] args) =>
		string.Format(Description, args).Trim();
	/// <summary>
	/// Formats the diagnostic.
	/// </summary>
	/// <param name="args">The arguments to format the description with.</param>
	/// <returns>The formatted diagnostic string.</returns>
	public string FormatDiagnostic(params object?[] args) =>
		$"{Level.ToDisplayString()} {ErrorCode}: {FormatDescription(args)}";

	public override string ToString() =>
		$"{Level.ToDisplayString()} {ErrorCode}";

}
