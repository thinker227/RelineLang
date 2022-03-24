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
	/// <typeparam name="TSyntax">The type of the syntax of the symbol
	/// to get or create.</typeparam>
	/// <typeparam name="TSymbol">The type of the symbol to get or create.</typeparam>
	/// <param name="syntax">The syntax of the symbol.</param>
	/// <param name="factory">The factory function to use to create a new symbol.</param>
	/// <returns>A new <typeparamref name="TSymbol"/> created by invoking
	/// <paramref name="factory"/> with <paramref name="syntax"/>,
	/// or a cached node.</returns>
	TSymbol GetSymbol<TSyntax, TSymbol>(TSyntax syntax, Func<TSyntax, TSymbol> factory) where TSyntax : ISyntaxNode where TSymbol : ISymbol;
	/// <summary>
	/// Gets a symbol corresponding to an identifier.
	/// </summary>
	/// <param name="identifier">The identifier to get the symbol of.</param>
	/// <returns>A <see cref="IIdentifiableSymbol"/> corresponding to
	/// <paramref name="identifier"/>, or <see langword="null"/> if none was found.</returns>
	IIdentifiableSymbol? GetIdentifier(string identifier);

}

internal static class BindingContextExtensions {

	/// <summary>
	/// Gets or creates a symbol from a specified <see cref="ISyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TSymbol">The type of the <see cref="SymbolNode"/>
	/// to get or create.</typeparam>
	/// <param name="syntax">The syntax of the symbol.</param>
	/// <returns>A new <typeparamref name="TSymbol"/> instantiated
	/// using <paramref name="syntax"/> as <see cref="SymbolNode.Syntax"/>,
	/// or a cached node.</returns>
	public static TSymbol GetSymbol<TSymbol>(this IBindingContext context, ISyntaxNode syntax)
		where TSymbol : SymbolNode, new() =>
		context.GetSymbol(syntax, static s => new TSymbol() { Syntax = s });

}
