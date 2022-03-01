namespace Reline.Compilation.Symbols;

public static class SymbolContextExtensions {

	/// <summary>
	/// Gets a parent node of a specified type of an <see cref="ISymbol"/>.
	/// </summary>
	/// <typeparam name="TParent">The type of the parent node to get.</typeparam>
	/// <param name="node">The <see cref="ISymbol"/> to get the parent of.</param>
	/// <returns>An <see cref="ISymbol"/> of type <typeparamref name="TParent"/>,
	/// or <see langword="null"/> if none was found.</returns>
	public static TParent? GetParentOfType<TParent>(this ISymbolContext context, ISymbol? node) where TParent : ISymbol => node switch {
		null => default,
		TParent p => p,
		_ => context.GetParentOfType<TParent>(context.GetParent(node))
	};

}
