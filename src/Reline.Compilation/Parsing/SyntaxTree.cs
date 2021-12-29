using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Parsing;

public sealed class SyntaxTree {

	public ProgramSyntax Root { get; }



	public SyntaxTree(ProgramSyntax root) {
		Root = root;
	}

}
