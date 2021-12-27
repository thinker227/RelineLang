using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Parsing;

public sealed class Parser {

	public IOperationResult<ImmutableArray<SyntaxToken>> Tokens { get; }



	public Parser(IOperationResult<ImmutableArray<SyntaxToken>> tokens) {
		Tokens = tokens;
	}



	public IOperationResult<int> ParseAll() {
		if (Tokens.HasErrors()) throw new CompilationException("Cannot parse invalid tokens.");
	}

}
