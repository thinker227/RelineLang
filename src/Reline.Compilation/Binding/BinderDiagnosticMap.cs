using System.Collections;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

/// <summary>
/// Maps diagnostics to symbols.
/// </summary>
internal sealed class BinderDiagnosticMap : IEnumerable<Diagnostic> {

	private readonly Dictionary<ISymbol, List<Diagnostic>> diagnostics;



	public BinderDiagnosticMap() {
		diagnostics = new();
	}



	/// <summary>
	/// Adds a diagnostic to a symbol.
	/// </summary>
	/// <param name="symbol"></param>
	/// <param name="diagnostic"></param>
	public void AddDiagnostic(ISymbol symbol, Diagnostic diagnostic) {
		if (diagnostics.TryGetValue(symbol, out var list)) {
			list.Add(diagnostic);
			return;
		}

		List<Diagnostic> newList = new() { diagnostic };
		diagnostics.Add(symbol, newList);
	}

	/// <summary>
	/// Gets the diagnostics associated with a specified symbol.
	/// </summary>
	/// <param name="symbol">The <see cref="ISymbol"/> to get the diagnostics of.</param>
	/// <returns>The diagnostics of <paramref name="symbol"/> if the symbol has
	/// any diagnostics associated with it.</returns>
	public IEnumerable<Diagnostic> GetDiagnostics(ISymbol symbol) {
		if (diagnostics.TryGetValue(symbol, out var list))
			return list;
		return Enumerable.Empty<Diagnostic>();
	}
	/// <summary>
	/// Gets the diagnostics of every symbol in the map.
	/// </summary>
	/// <returns>The diagnostics of every symbol in the map..</returns>
	public IEnumerator<Diagnostic> GetEnumerator() {
		foreach (var list in diagnostics.Values)
			foreach (var d in list)
				yield return d;
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
