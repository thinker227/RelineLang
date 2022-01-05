using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

public abstract class SymbolNode : ISymbol {

	public void Accept(IVisitor visitor) =>
		visitor.Visit(this);
	public T Accept<T>(IVisitor<T> visitor) =>
		visitor.Visit(this);

}
