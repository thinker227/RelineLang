using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Base for binders binding identifiers.
/// </summary>
internal abstract class BaseIdentifierBinder<TSymbol> where TSymbol : IIdentifiableSymbol {

	protected readonly Dictionary<string, TSymbol> symbols;



	public BaseIdentifierBinder() {
		symbols = new();
	}



	public virtual void RegisterSymbol(TSymbol symbol) {
		symbols.TryAdd(symbol.Identifier, symbol);
	}

	public virtual TSymbol? GetSymbol(string identifier) =>
		symbols.TryGetValue(identifier, out var symbol) ? symbol : default;

}
