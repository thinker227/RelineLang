using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding.Nodes;

/// <summary>
/// Represents a missing identifier.
/// </summary>
public sealed class MissingIdentifierSymbol : SymbolNode, IIdentifiableSymbol {

	public string Identifier { get; set; } = null!;

}
