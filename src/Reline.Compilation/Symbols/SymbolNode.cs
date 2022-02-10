using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an abstract symbol node.
/// </summary>
public abstract class SymbolNode : ISymbol {

	public ISyntaxNode? Syntax { get; init; }
	public ISymbol? Parent { get; init; }



	public void Accept(IVisitor<ISymbol> visitor) =>
		visitor.Visit(this);
	public TResult Accept<TResult>(IVisitor<ISymbol, TResult> visitor) =>
		visitor.Visit(this);

}
