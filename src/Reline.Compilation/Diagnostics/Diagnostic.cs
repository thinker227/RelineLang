﻿namespace Reline.Compilation.Diagnostics;

/// <summary>
/// Represents a diagnostic generated by compilation.
/// </summary>
public readonly record struct Diagnostic {

	/// <summary>
	/// The <see cref="DiagnosticDescription"/> of the diagnostic.
	/// </summary>
	public DiagnosticDescription Description { get; }
	/// <summary>
	/// The span in the source text of the diagnostic,
	/// or <see langword="null"/> if the diagnostic does not have a specific location.
	/// </summary>
	public TextSpan? Location { get; }
	/// <summary>
	/// The formatted description of the diagnostic.
	/// </summary>
	public string FormattedDescription { get; } = "No description";



	private Diagnostic(DiagnosticDescription description, TextSpan? location, string formattedDescription) {
		Description = description;
		Location = location;
		FormattedDescription = formattedDescription;
	}



	/// <summary>
	/// Creates a new <see cref="Diagnostic"/>.
	/// </summary>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="location">The span of the diagnostic in the source text.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	/// <returns>A new <see cref="Diagnostic"/>.</returns>
	public static Diagnostic Create(DiagnosticDescription description, TextSpan? location, params object?[] formatArgs) {
		string formattedDescription = description.FormatDescription(formatArgs);
		return new Diagnostic(description, location, formattedDescription);
	}

	public override string ToString() {
		string locationString = Location is not null ? $"({Location}) " : "";
		return $"{locationString}{Description.ErrorCode}: {FormattedDescription}";
	}

}
