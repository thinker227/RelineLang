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



	public virtual void RegisterSymbol(TSymbol symbol) =>
		symbols.TryAdd(symbol.Identifier, symbol);

	public virtual TSymbol? GetSymbol(string identifier) =>
		symbols.TryGetValue(identifier, out var symbol) ? symbol : default;

	public virtual bool IsDefined(string identifier) =>
		symbols.ContainsKey(identifier);

	public virtual IEnumerator<TSymbol> GetEnumerator() =>
		symbols.Values.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

}
