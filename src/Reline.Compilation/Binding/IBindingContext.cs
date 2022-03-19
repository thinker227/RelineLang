using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

/// <summary>
/// A context for binding symbols.
/// </summary>
internal interface IBindingContext : ISymbolContext {

	/// <summary>
	/// Gets or creates a symbol from a specified <see cref="ISyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TSymbol">The type of the symbol to create.</typeparam>
	/// <param name="syntax">The syntax of the symbol.</param>
	/// <returns>A new <typeparamref name="TSymbol"/> or a cached node.</returns>
	TSymbol GetSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new();
	/// <summary>
	/// Adds a diagnostic to the binding context.
	/// </summary>
	/// <param name="location">The location of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	void AddDiagnostic(TextSpan? location, DiagnosticDescription description, params object?[] formatArgs);
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
	/// Adds a diagnostic to a binding context.
	/// </summary>
	/// <param name="context">The <see cref="IBindingContext"/>
	/// to add the diagnostic to.</param>
	/// <param name="token">The <see cref="SyntaxToken"/> to add the diagnostic to.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public static void AddDiagnostic(this IBindingContext context, SyntaxToken token, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(token.Span, description, formatArgs);
	/// <summary>
	/// Adds a diagnostic to a binding context.
	/// </summary>
	/// <param name="context">The <see cref="IBindingContext"/>
	/// to add the diagnostic to.</param>
	/// <param name="syntax">The <see cref="ISyntaxNode"/> to add the diagnostic to.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public static void AddDiagnostic(this IBindingContext context, ISyntaxNode syntax, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(syntax.GetTextSpan(), description, formatArgs);
	/// <summary>
	/// Adds a diagnostic to a binding context.
	/// </summary>
	/// <param name="context">The <see cref="IBindingContext"/>
	/// to add the diagnostic to.</param>
	/// <param name="syntax">The <see cref="ISymbol"/> to add the diagnostic to.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public static void AddDiagnostic(this IBindingContext context, ISymbol symbol, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(symbol.Syntax?.GetTextSpan(), description, formatArgs);

}
