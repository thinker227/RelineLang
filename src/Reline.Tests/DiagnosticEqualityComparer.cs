using Reline.Compilation.Diagnostics;

namespace Reline.Tests;

public class DiagnosticEqualityComparer : IEqualityComparer<Diagnostic> {

	public DiagnosticComparison Comparison { get; }



	public DiagnosticEqualityComparer(DiagnosticComparison comparison) {
		Comparison = comparison;
	}



	public bool Equals(Diagnostic x, Diagnostic y) =>
		(Comparison.HasFlag(DiagnosticComparison.IgnoreLocation) ||
		x.Location == y.Location) &&

		(Comparison.HasFlag(DiagnosticComparison.IgnoreDescription) ||
		x.Description == y.Description) &&

		(Comparison.HasFlag(DiagnosticComparison.IgnoreFormatting) ||
		x.FormattedDescription == y.FormattedDescription);

	public int GetHashCode(Diagnostic obj) => throw new NotSupportedException();

}

[Flags]
public enum DiagnosticComparison {
	IgnoreLocation = 1,
	IgnoreDescription = 2,
	IgnoreFormatting = 4,
}
