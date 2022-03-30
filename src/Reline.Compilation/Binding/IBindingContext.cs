using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

/// <summary>
/// A context for binding symbols.
/// </summary>
internal interface IBindingContext : ISemanticContext, ILineContext, ISymbolCreationContext, IIdentifierContext, IDiagnosticContext { }

/// <summary>
/// A context for binding and getting identifiers.
/// </summary>
internal interface IIdentifierContext {

	/// <summary>
	/// The binder for label symbols.
	/// </summary>
	IIdentifierBinder<LabelSymbol> LabelBinder { get; }
	/// <summary>
	/// The binder for variable symbols.
	/// </summary>
	IIdentifierBinder<VariableSymbol> VariableBinder { get; }
	/// <summary>
	/// The binder for parameter symbols.
	/// </summary>
	IScopedIdentifierBinder<ParameterSymbol> ParameterBinder { get; }
	/// <summary>
	/// The binder for function symbols.
	/// </summary>
	IIdentifierBinder<IFunctionSymbol> FunctionBinder { get; }

}

/// <summary>
/// A context for creating and getting symbols.
/// </summary>
internal interface ISymbolCreationContext {

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

}

/// <summary>
/// A context for getting information about line spans.
/// </summary>
internal interface ILineContext {

	int StartLine { get; }
	int EndLine { get; }

}

internal static class ContextExtensions {

	/// <summary>
	/// Gets or creates a symbol from a specified <see cref="ISyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TSymbol">The type of the <see cref="SymbolNode"/>
	/// to get or create.</typeparam>
	/// <param name="syntax">The syntax of the symbol.</param>
	/// <returns>A new <typeparamref name="TSymbol"/> instantiated
	/// using <paramref name="syntax"/> as <see cref="SymbolNode.Syntax"/>,
	/// or a cached node.</returns>
	public static TSymbol GetSymbol<TSymbol>(this ISymbolCreationContext context, ISyntaxNode syntax) where TSymbol : SymbolNode, new() =>
		context.GetSymbol(syntax, static s => new TSymbol() { Syntax = s });

	/// <summary>
	/// Gets a symbol corresponding to an identifier.
	/// </summary>
	/// <param name="context">The source context.</param>
	/// <param name="identifier">The identifier to get the symbol of.</param>
	/// <param name="location">The location to get the identifier within.</param>
	/// <returns>A <see cref="IIdentifiableSymbol"/> corresponding to
	/// <paramref name="identifier"/> within <paramref name="location"/>,
	/// or <see langword="null"/> if none was found.</returns>
	public static IIdentifiableSymbol? GetIdentifier(this IIdentifierContext context, string identifier, int? location = null) =>
		context.LabelBinder.GetSymbol(identifier) ??
		context.VariableBinder.GetSymbol(identifier) ??
		(location.HasValue ? context.ParameterBinder.GetSymbol(identifier, location.Value) : null) ??
		context.FunctionBinder.GetSymbol(identifier) ??
		(IIdentifiableSymbol?)null;
	/// <summary>
	/// Gets whether a symbol with the specified identifier
	/// is bound within a <see cref="IIdentifierContext"/>.
	/// </summary>
	/// <param name="context">The source context.</param>
	/// <param name="identifier">The identifier to get.</param>
	/// <param name="location">The location to get the identifier within.</param>
	/// <returns>Whether a symbol with the identifier
	/// <paramref name="identifier"/> and scope <paramref name="location"/>
	/// is bound within the context.</returns>
	public static bool IsDefined(this IIdentifierContext context, string identifier, int? location = null) =>
		context.GetIdentifier(identifier, location) is not null;

}
