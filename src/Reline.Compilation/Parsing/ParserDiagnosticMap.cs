using System.Collections;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Parsing;

internal sealed class ParserDiagnosticMap : IEnumerable<Diagnostic> {

	private readonly Dictionary<SyntaxToken, List<Diagnostic>> tokenMap;
	private readonly Dictionary<ISyntaxNode, List<Diagnostic>> nodeMap;



	public ParserDiagnosticMap() {
		tokenMap = new();
		nodeMap = new();
	}



	public void AddDiagnostic(SyntaxToken token, Diagnostic diagnostic) {
		if (tokenMap.TryGetValue(token, out var list)) {
			list.Add(diagnostic);
			return;
		}

		List<Diagnostic> newList = new() { diagnostic };
		tokenMap.Add(token, newList);
	}
	public void AddDiagnostic(ISyntaxNode node, Diagnostic diagnostic) {
		if (nodeMap.TryGetValue(node, out var list)) {
			list.Add(diagnostic);
			return;
		}

		List<Diagnostic> newList = new() { diagnostic };
		nodeMap.Add(node, newList);
	}

	public IEnumerator<Diagnostic> GetEnumerator() {
		foreach (var list in tokenMap.Values)
			foreach (var d in list)
				yield return d;
		foreach (var list in nodeMap.Values)
			foreach (var d in list)
				yield return d;
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
