using System.Linq;
using Reline.Compilation.Syntax;
using Reline.Compilation.Symbols;
using Reline.Compilation.Binding;

namespace Reline.Tests;

/// <summary>
/// Contains utilities for binder tests.
/// </summary>
internal static class BinderTestUtilities {

	private static SemanticModel GetModel(string source) {
		var syntaxTree = AssertAsync.CompletesIn(2000, () => SyntaxTree.ParseString(source));
		var model = AssertAsync.CompletesIn(2000, () => SemanticModel.BindTree(syntaxTree));
		return model;
	}

	/// <summary>
	/// Gets a <see cref="NodeTestRunner{TNode}"/> and <see cref="SemanticModel"/>
	/// compiled from a source string.
	/// </summary>
	/// <param name="source">The source text.</param>
	/// <returns>A <see cref="NodeTestRunner{TNode}"/> of <see cref="ISymbol"/> and
	/// a <see cref="SemanticModel"/> created from <paramref name="source"/>.</returns>
	public static CompileResult Compile(string source) {
		var model = GetModel(source);
		var nodes = model.Root.GetDescendants()
			.Prepend(model.Root)
			.Select(n => (ISymbol)n);
		NodeTestRunner<ISymbol> runner = new(nodes);

		return new(runner, model);
	}



	public readonly record struct CompileResult(NodeTestRunner<ISymbol> Runner, SemanticModel Model);

}
