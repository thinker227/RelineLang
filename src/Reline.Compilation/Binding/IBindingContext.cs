using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

/// <summary>
/// A context for binding symbols.
/// </summary>
internal interface IBindingContext : ISemanticContext, IDiagnosticContext {

	/// <summary>
	/// Gets or creates a symbol from a specified <see cref="ISyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TSymbol">The type of the symbol to create.</typeparam>
	/// <param name="syntax">The syntax of the symbol.</param>
	/// <returns>A new <typeparamref name="TSymbol"/> or a cached node.</returns>
	TSymbol GetSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new();
	/// <summary>
	/// Gets a symbol corresponding to an identifier.
	/// </summary>
	/// <param name="identifier">The identifier to get the symbol of.</param>
	/// <returns>A <see cref="IIdentifiableSymbol"/> corresponding to
	/// <paramref name="identifier"/>, or <see langword="null"/> if none was found.</returns>
	IIdentifiableSymbol? GetIdentifier(string identifier);

}
