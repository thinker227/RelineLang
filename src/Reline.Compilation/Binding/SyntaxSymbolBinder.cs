using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using System.Diagnostics.CodeAnalysis;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds <see cref="ISyntaxNode"/>s and <see cref="ISymbol"/>s.
/// </summary>
/// <remarks>
/// This is mainly used for linking syntax with symbols
/// and to avoid creating duplicate symbols for the same syntax.
/// </remarks>
internal sealed class SyntaxSymbolBinder {

	private readonly Dictionary<ISyntaxNode, ISymbol> symbols;



	public SyntaxSymbolBinder() {
		symbols = new();
	}



	/// <summary>
	/// Tries to get a <see cref="ISymbol"/> from a <see cref="ISyntaxNode"/>.
	/// </summary>
	public bool TryGetSymbol(ISyntaxNode syntax, [NotNullWhen(true)] out ISymbol? symbol) =>
		symbols.TryGetValue(syntax, out symbol);
	/// <summary>
	/// Binds a <see cref="ISyntaxNode"/> to a <see cref="ISymbol"/>.
	/// </summary>
	public void Bind(ISyntaxNode syntax, ISymbol symbol) =>
		symbols.TryAdd(syntax, symbol);

}
