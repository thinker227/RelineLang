using System.Linq;
using Reline.Compilation.Syntax;
using Reline.Compilation.Symbols;

namespace Reline.Tests;

public abstract class BinderTestBase {

	/// <summary>
	/// Gets a <see cref="NodeTestRunner{TNode}"/> and <see cref="SemanticModel"/>
	/// compiled from a source string.
	/// </summary>
	/// <param name="source">The source text.</param>
	/// <returns>A <see cref="NodeTestRunner{TNode}"/> of <see cref="ISymbol"/> and
	/// a <see cref="SemanticModel"/> created from <paramref name="source"/>.</returns>
	protected static (NodeTestRunner<ISymbol>, SemanticModel) Compile(string source) {
		var syntaxTree = AssertAsync.CompletesIn(2000, () => SyntaxTree.ParseString(source));
		var tree = AssertAsync.CompletesIn(2000, () => SemanticModel.BindTree(syntaxTree));
		var nodes = tree.Root.GetDescendants()
			.Prepend(tree.Root)
			.Select(n => (ISymbol)n);
		NodeTestRunner<ISymbol> runner = new(nodes);
		return (runner, tree);
	}

}
