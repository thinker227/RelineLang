using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Base for binders binding identifiers.
/// </summary>
internal sealed class IdentifierBinder<TSymbol> where TSymbol : IIdentifiableSymbol {

	private readonly Dictionary<string, TSymbol> symbols;



	public IdentifierBinder() {
		symbols = new();
	}



	public void RegisterSymbol(TSymbol symbol) =>
		symbols.TryAdd(symbol.Identifier, symbol);

	public TSymbol? GetSymbol(string identifier) =>
		symbols.TryGetValue(identifier, out var symbol) ? symbol : default;

	public bool IsDefined(string identifier) =>
		symbols.ContainsKey(identifier);

}
