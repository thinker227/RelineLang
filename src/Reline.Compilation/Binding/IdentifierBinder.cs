using System.Collections;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Defines a binder for identifiers.
/// </summary>
/// <typeparam name="TSymbol">The type of the bound identifiable symbols.</typeparam>
internal interface IIdentifierBinder<TSymbol> : IReadOnlyCollection<TSymbol> where TSymbol : IIdentifiableSymbol {

	/// <summary>
	/// Registers a symbol in the binder.
	/// </summary>
	/// <param name="symbol">The symbol to register.</param>
	void RegisterSymbol(TSymbol symbol);
	/// <summary>
	/// Gets a symbol with the specified identifier from the binder
	/// or <see langword="null"/> if no symbol with the identifier is bound.
	/// </summary>
	/// <param name="identifier">The identifier of the symbol to get.</param>
	/// <returns>A symbol with the identifier <paramref name="identifier"/>
	/// or <see langword="null"/> if none is bound.</returns>
	TSymbol? GetSymbol(string identifier);

}

/// <summary>
/// Defines a binder for scoped identifiers.
/// </summary>
/// <typeparam name="TSymbol">The type of the bound scoped identifiable symbols.</typeparam>
internal interface IScopedIdentifierBinder<TSymbol> : IReadOnlyCollection<TSymbol> where TSymbol : IScopedIdentifiableSymbol {

	/// <summary>
	/// Registers a symbol in the binder.
	/// </summary>
	/// <param name="symbol">The symbol to register.</param>
	void RegisterSymbol(TSymbol symbol);
	/// <summary>
	/// Gets a symbol with the specified identifier and scope from the binder
	/// or <see langword="null"/> if no symbol with the identifier and scope is bound.
	/// </summary>
	/// <param name="identifier">The identifier of the symbol to get.</param>
	/// <returns>A symbol with the identifier <paramref name="identifier"/>
	/// and scope <paramref name="location"/>
	/// or <see langword="null"/> if none is bound.</returns>
	TSymbol? GetSymbol(string identifier, int location);

}

internal static class IdentifierBinderExtensions {

	/// <summary>
	/// Registers a range of symbols in an identifier binder.
	/// </summary>
	/// <param name="binder">The source binder.</param>
	/// <param name="symbols">The symbols to register.</param>
	public static void RegisterRange<TSymbol>(this IIdentifierBinder<TSymbol> binder, IEnumerable<TSymbol> symbols) where TSymbol : IIdentifiableSymbol {
		foreach (var symbol in symbols) binder.RegisterSymbol(symbol);
	}
	/// <summary>
	/// Registers a range of symbols in a scoped identifier binder.
	/// </summary>
	/// <param name="binder">The source binder.</param>
	/// <param name="symbols">The symbols to register.</param>
	public static void RegisterRange<TSymbol>(this IScopedIdentifierBinder<TSymbol> binder, IEnumerable<TSymbol> symbols) where TSymbol : IScopedIdentifiableSymbol {
		foreach (var symbol in symbols) binder.RegisterSymbol(symbol);
	}

	/// <summary>
	/// Gets whether a symbol with the specified identifier is bound within an identifier binder.
	/// </summary>
	/// <param name="binder">The source binder.</param>
	/// <param name="identifier">The identifier to get.</param>
	/// <returns>Whether a symbol with the identifier
	/// <paramref name="identifier"/> is bound within the binder.</returns>
	public static bool IsDefined<TSymbol>(this IIdentifierBinder<TSymbol> binder, string identifier) where TSymbol : IIdentifiableSymbol =>
		binder.GetSymbol(identifier) is not null;
	/// <summary>
	/// Gets whether a symbol with the specified identifier is bound within a scoped identifier binder.
	/// </summary>
	/// <param name="binder">The source binder.</param>
	/// <param name="identifier">The identifier to get.</param>
	/// <returns>Whether a symbol with the identifier
	/// <paramref name="identifier"/> is bound within the binder.</returns>
	public static bool IsDefined<TSymbol>(this IScopedIdentifierBinder<TSymbol> binder, string identifier, int location) where TSymbol : IScopedIdentifiableSymbol =>
		binder.GetSymbol(identifier, location) is not null;

}

/// <summary>
/// Base for binders binding identifiers.
/// </summary>
internal class IdentifierBinder<TSymbol> : IIdentifierBinder<TSymbol> where TSymbol : IIdentifiableSymbol {

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
	/// Registers a range of symbols in the binder.
	/// </summary>
	/// <param name="symbols">The symbols to register.</param>
	public void RegisterRange(IEnumerable<TSymbol> symbols) {
		foreach (var symbol in symbols) RegisterSymbol(symbol);
	}

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
