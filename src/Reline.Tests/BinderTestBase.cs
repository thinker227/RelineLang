using System.Linq;
using Reline.Compilation.Parsing;
using Reline.Compilation.Binding;
using Reline.Compilation.Symbols;

namespace Reline.Tests;

public abstract class BinderTestBase : TreeTestBase<ISymbol> {

	private IEnumerable<ISymbol>? enumerable;

	/// <summary>
	/// Sets the tree to traverse from a source string.
	/// </summary>
	/// <param name="source">The source stirng to generate the tree from.</param>
	protected SemanticModel SetTree(string source) {
		var syntaxTree = AssertAsync.CompletesIn(2000, () => Parser.ParseString(source));
		var semanticTree = AssertAsync.CompletesIn(2000, () => Binder.BindTree(syntaxTree));
		var symbols = semanticTree.Root.GetDescendants();
		enumerable = symbols
			.Prepend(semanticTree.Root)
			.Select(s => (ISymbol)s);
		return semanticTree;
	}

	protected override IEnumerable<ISymbol>? GetEnumerable() =>
		enumerable;
	protected override void Reset() =>
		enumerable = null;

}
