using Reline.Compilation.Syntax;

namespace Reline.Compilation.Symbols;

/// <summary>
/// A context for semantic information.
/// </summary>
public interface ISemanticContext : ISyntaxContext {

	/// <summary>
	/// The root node of the context.
	/// </summary>
	ProgramSymbol Root { get; }

	/// <summary>
	/// Gets the parent node of a specified <see cref="ISymbol"/>.
	/// </summary>
	/// <param name="symbol">The <see cref="ISymbol"/>
	/// to get the parent of.</param>
	/// <returns>The parent of <paramref name="symbol"/>, or <see langword="null"/>
	/// if the node is the root of the context.</returns>
	ISymbol? GetParent(ISymbol symbol);

}

/// <summary>
/// A context containing a <see cref="Syntax.SyntaxTree"/>.
/// </summary>
public interface ISyntaxContext {

	/// <summary>
	/// The <see cref="Syntax.SyntaxTree"/> which created the context.
	/// </summary>
	SyntaxTree SyntaxTree { get; }

}
