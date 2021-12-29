using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Parsing;

public readonly record struct ParseResult(
	ImmutableArray<Diagnostic> Diagnostics,
	SyntaxTree SyntaxTree
) : IOperationResult<SyntaxTree> {

	SyntaxTree IOperationResult<SyntaxTree>.Result => SyntaxTree;

}
