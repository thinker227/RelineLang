using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Diagnostics;

/// <summary>
/// A context for diagnostics.
/// </summary>
public interface IDiagnosticContext {

	/// <summary>
	/// Adds a diagnostic to the context.
	/// </summary>
	/// <param name="location">The location of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	void AddDiagnostic(TextSpan? location, DiagnosticDescription description, params object?[] formatArgs);

}

public static class DiagnosticContextExtensions {

	/// <summary>
	/// Adds a diagnostic to a diagnostic context.
	/// </summary>
	/// <param name="context">The context to add the diagnostic to.</param>
	/// <param name="token">The <see cref="SyntaxToken"/>
	/// to get the diagnostic span from.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public static void AddDiagnostic(this IDiagnosticContext context, SyntaxToken token, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(token.Span, description, formatArgs);
	/// <summary>
	/// Adds a diagnostic to a diagnostic context.
	/// </summary>
	/// <param name="context">The context to add the diagnostic to.</param>
	/// <param name="syntax">The <see cref="ISyntaxNode"/>
	/// to get the diagnostic span from.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public static void AddDiagnostic(this IDiagnosticContext context, ISyntaxNode syntax, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(syntax.GetTextSpan(), description, formatArgs);
	/// <summary>
	/// Adds a diagnostic to a diagnostic context.
	/// </summary>
	/// <param name="context">The context to add the diagnostic to.</param>
	/// <param name="symbol">The <see cref="ISymbol"/>
	/// to get the diagnostic span from.</param>
	/// <param name="description">The description of the diagnostic.</param>
	/// <param name="formatArgs">The arguments to format the description with.</param>
	public static void AddDiagnostic(this IDiagnosticContext context, ISymbol symbol, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(symbol.Syntax?.GetTextSpan(), description, formatArgs);

}
