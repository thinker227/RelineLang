using System;
using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax;

namespace Reline.CommandLine;

internal abstract record CompilationResult {

	public sealed record Success(
		SyntaxTree SyntaxTree,
		SemanticModel SemanticModel
	) : CompilationResult;

	public sealed record InternalException(
		Exception Exception
	) : CompilationResult;

	public sealed record Timeout() : CompilationResult;

}
