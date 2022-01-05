using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

public static class SymbolExtensions {

	public static TNode GetSyntaxAs<TNode>(this ISymbol symbol) where TNode : ISyntaxNode =>
		(TNode)symbol.Syntax;

}
