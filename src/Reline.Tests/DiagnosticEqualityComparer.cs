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

		(Comparison.HasFlag(DiagnosticComparison.IgnoreInternalDescription) ||
		x.InternalDescription == y.InternalDescription) &&

		(Comparison.HasFlag(DiagnosticComparison.IgnoreFormatting) ||
		x.Description == y.Description);

	public int GetHashCode(Diagnostic obj) => throw new NotSupportedException();

}

[Flags]
public enum DiagnosticComparison {
	IgnoreLocation = 1,
	IgnoreInternalDescription = 2,
	IgnoreFormatting = 4,
}
