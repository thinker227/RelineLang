using System.Threading;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax;

public sealed class SyntaxTree {

	private IReadOnlyDictionary<ISyntaxNode, ISyntaxNode?>? parents;


	
	public ImmutableArray<Diagnostic> Diagnostics { get; }
	public ProgramSyntax Root { get; }



	internal SyntaxTree(ProgramSyntax root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
	}



	public ISyntaxNode? GetParent(ISyntaxNode node) {
		if (parents is null) {
			var initializedParents = CreateParentsDictionary(Root);
			Interlocked.CompareExchange(ref parents, initializedParents, null);
		}
		
		return parents[node];
	}
	private static IReadOnlyDictionary<ISyntaxNode, ISyntaxNode?> CreateParentsDictionary(ISyntaxNode root) {
		Dictionary<ISyntaxNode, ISyntaxNode?> result = new();
		result.Add(root, null);
		AddParentsToDictionary(result, root);
		return result;
	}
	private static void AddParentsToDictionary(IDictionary<ISyntaxNode, ISyntaxNode?> dictionary, ISyntaxNode node) {
		var children = node.GetChildren();
		foreach (var child in children) {
			dictionary.Add(child, node);
			AddParentsToDictionary(dictionary, child);
		}
	}

}
