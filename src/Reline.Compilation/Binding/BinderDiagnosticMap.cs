using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

internal sealed class BinderDiagnosticMap {

	private readonly Dictionary<ISymbol, List<Diagnostic>> diagnostics;



	public BinderDiagnosticMap() {
		diagnostics = new();
	}



	public void AddDiagnostic(ISymbol symbol, Diagnostic diagnostic) {
		if (diagnostics.TryGetValue(symbol, out var list)) {
			list.Add(diagnostic);
			return;
		}

		List<Diagnostic> newList = new() { diagnostic };
		diagnostics.Add(symbol, newList);
	}

	public IEnumerable<Diagnostic> GetDiagnostics() {
		foreach (var list in diagnostics.Values)
			foreach (var d in list)
				yield return d;
	}

}
