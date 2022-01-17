using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

public readonly record struct SymbolCompilationResult(ImmutableArray<Diagnostic> Diagnostics, SymbolTree Tree) : IOperationResult<SymbolTree> {

	SymbolTree IOperationResult<SymbolTree>.Result => Tree;

}
