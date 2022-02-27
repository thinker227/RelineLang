using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax;

public sealed class SyntaxTree {

	public ImmutableArray<Diagnostic> Diagnostics { get; }
	public ProgramSyntax Root { get; }



	private SyntaxTree(ProgramSyntax root, ImmutableArray<Diagnostic> diagnostics) {
		Root = root;
		Diagnostics = diagnostics;
	}

}
