using System.Collections;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Base for binders binding identifiers.
/// </summary>
internal class IdentifierBinder<TSymbol> : IReadOnlyCollection<TSymbol> where TSymbol : IIdentifiableSymbol {

	protected readonly Dictionary<string, TSymbol> symbols;



	public int Count => symbols.Count;



	public IdentifierBinder() {
		symbols = new();
	}



	/// <summary>
	/// Registers a symbol in the binder.
	/// </summary>
	/// <param name="symbol">The symbol to register.</param>
	public virtual void RegisterSymbol(TSymbol symbol) =>
		symbols.TryAdd(symbol.Identifier, symbol);

	/// <summary>
	/// Gets a symbol with the specified identifier from the binder
	/// or <see langword="null"/> if no symbol with the identifier is not bound.
	/// </summary>
	/// <param name="identifier">The identifier of the symbol to get.</param>
	/// <returns>A symbol with the identifier <paramref name="identifier"/>
	/// or <see langword="null"/> if none is bound.</returns>
	public virtual TSymbol? GetSymbol(string identifier) =>
		symbols.TryGetValue(identifier, out var symbol) ? symbol : default;

	/// <summary>
	/// Gets whether a symbol with the specified identifier is bound within the binder.
	/// </summary>
	/// <param name="identifier">The identifier to get.</param>
	/// <returns>Whether a symbol with the identifier
	/// <paramref name="identifier"/> is bound within the binder.</returns>
	public virtual bool IsDefined(string identifier) =>
		symbols.ContainsKey(identifier);

	public virtual IEnumerator<TSymbol> GetEnumerator() =>
		symbols.Values.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

}
