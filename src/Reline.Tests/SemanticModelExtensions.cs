using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Tests;

public static class SemanticModelExtensions {

	/// <summary>
	/// Asserts that a semantic model has a specified diagnostic
	/// at a specific location.
	/// </summary>
	/// <param name="model">The source semantic model.</param>
	/// <param name="location">The location of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	public static void HasDiagnostic(this SemanticModel model, TextSpan? location, DiagnosticDescription description) {
		Assert.Contains(model.Diagnostics, d =>
			d.InternalDescription == description);
		Assert.Contains(model.Diagnostics, d =>
			d.Location == location);
	}
	/// <summary>
	/// Asserts that a semantic model has a specified diagnostic
	/// at the location of a specified <see cref="SyntaxToken"/>.
	/// </summary>
	/// <param name="model">The source semantic model.</param>
	/// <param name="token">The token of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	public static void HasDiagnostic(this SemanticModel model, SyntaxToken token, DiagnosticDescription description) =>
		model.HasDiagnostic(token.Span, description);
	/// <summary>
	/// Asserts that a semantic model has a specified diagnostic
	/// at the location of a specified <see cref="ISyntaxNode"/>.
	/// </summary>
	/// <param name="model">The source semantic model.</param>
	/// <param name="syntax">The syntax of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	public static void HasDiagnostic(this SemanticModel model, ISyntaxNode syntax, DiagnosticDescription description) =>
		model.HasDiagnostic(syntax.GetTextSpan(), description);
	/// <summary>
	/// Asserts that a semantic model has a specified diagnostic
	/// at the location of a specified <see cref="ISymbol"/>.
	/// </summary>
	/// <param name="model">The source semantic model.</param>
	/// <param name="symbol">The symbol of the diagnostic.</param>
	/// <param name="description">The description of the diagnostic.</param>
	public static void HasDiagnostic(this SemanticModel model, ISymbol symbol, DiagnosticDescription description) =>
		model.HasDiagnostic(symbol.Syntax?.GetTextSpan(), description);

}
