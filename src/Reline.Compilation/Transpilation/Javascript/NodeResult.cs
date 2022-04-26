using Reline.Compilation.Symbols;

namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// The result of a Javascript transpilation operation targeting Node.js.
/// </summary>
public sealed class NodeResult : IJavscriptTranspilationResult {

	public ISemanticContext SemanticContext { get; }
	public JavascriptTarget Target => JavascriptTarget.Node;
	IEnumerable<ITranspiledSource> ITranspilationResult.Sources =>
		new ITranspiledSource[] { Source };
	/// <summary>
	/// The Javascript source.
	/// </summary>
	public JavascriptSource Source { get; }



	internal NodeResult(ISemanticContext semanticContext, string source) {
		SemanticContext = semanticContext;
		Source = new(source);
	}

}
