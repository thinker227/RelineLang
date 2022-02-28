using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a symbol.
/// </summary>
public interface ISymbol : INode<ISymbol> {

	/// <summary>
	/// The <see cref="ISyntaxNode"/> this symbol was created from.
	/// </summary>
	ISyntaxNode? Syntax { get; }

}
