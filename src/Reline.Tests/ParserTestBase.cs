using System.Linq;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests;

public abstract class ParserTestBase {

	/// <summary>
	/// Gets a <see cref="NodeTestRunner{TNode}"/> and <see cref="SyntaxTree"/>
	/// parsed from a source string.
	/// </summary>
	/// <param name="source">The source text.</param>
	/// <returns>A <see cref="NodeTestRunner{TNode}"/> of <see cref="ISyntaxNode"/> and
	/// a <see cref="SyntaxTree"/> created from <paramref name="source"/>.</returns>
	protected static (NodeTestRunner<ISyntaxNode>, SyntaxTree) Parse(string source) {
		var tree = AssertAsync.CompletesIn(2000, () => SyntaxTree.ParseString(source));
		var nodes = tree.Root.GetDescendants()
			.Prepend(tree.Root)
			.Select(n => (ISyntaxNode)n);
		NodeTestRunner<ISyntaxNode> runner = new(nodes);
		return (runner, tree);
	}

}
