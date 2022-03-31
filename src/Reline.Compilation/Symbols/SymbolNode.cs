using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an abstract symbol node.
/// </summary>
public abstract class SymbolNode : ISymbol {

	public ISyntaxNode? Syntax { get; init; }

	public abstract IEnumerable<ISymbol> GetChildren();

}
