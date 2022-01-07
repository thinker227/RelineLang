using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

public abstract class SymbolNode : ISymbol {

	public void Accept(IVisitor<ISymbol> visitor) =>
		visitor.Visit(this);
	public TResult Accept<TResult>(IVisitor<ISymbol, TResult> visitor) =>
		visitor.Visit(this);

}
