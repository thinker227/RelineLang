using System.Linq;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests;

public abstract class ParserTestBase : TreeTestBase<ISyntaxNode> {

	private IEnumerable<ISyntaxNode>? enumerable;

	/// <summary>
	/// Sets the tree to traverse from a source string.
	/// </summary>
	/// <param name="source">The source stirng to generate the tree from.</param>
	protected void SetTree(string source) {
		var tree = AssertAsync.CompletesIn(2000, () => Parser.ParseString(source));
		var nodes = tree.Root.GetDescendants();
		enumerable = nodes
			.Prepend(tree.Root) // Test for ProgramSyntax
			.Select(n => (ISyntaxNode)n);
	}

	protected override IEnumerable<ISyntaxNode>? GetEnumerable() =>
		enumerable;
	protected override void Reset() =>
		enumerable = null;

}
