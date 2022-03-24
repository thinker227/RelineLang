namespace Reline.Compilation.Symbols;

public static class SymbolContextExtensions {

	/// <summary>
	/// Gets an ancestor node of a specified type of an <see cref="ISymbol"/>.
	/// </summary>
	/// <typeparam name="TAncestor">The type of the ancestor node to get.</typeparam>
	/// <param name="context">The <see cref="ISemanticContext"/>
	/// use as the context for the nodes.</param>
	/// <param name="node">The <see cref="ISymbol"/> to get the ancestor of.</param>
	/// <returns>An <see cref="ISymbol"/> of type <typeparamref name="TAncestor"/>,
	/// or <see langword="null"/> if none was found.</returns>
	public static TAncestor? GetAncestor<TAncestor>(this ISemanticContext context, ISymbol? node) where TAncestor : ISymbol => node switch {
		null => default,
		TAncestor a => a,
		_ => context.GetAncestor<TAncestor>(context.GetParent(node))
	};

}
