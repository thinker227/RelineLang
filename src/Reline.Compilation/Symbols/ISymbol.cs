using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a symbol.
/// </summary>
public interface ISymbol {

	/// <summary>
	/// The <see cref="ISyntaxNode"/> this symbol was created from.
	/// </summary>
	ISyntaxNode? Syntax { get; }

	/// <summary>
	/// Gets the child nodes of the symbol.
	/// </summary>
	/// <returns>A collection of children symbol nodes.</returns>
	IEnumerable<ISymbol> GetChildren();

}
