using Reline.Compilation.Symbols;

namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// The result of a Javascript transpilation operation targeting a web browser.
/// </summary>
public sealed class WebResult : IJavscriptTranspilationResult {

	public ISemanticContext SemanticContext { get; }
	public JavascriptTarget Target => JavascriptTarget.Web;
	/// <summary>
	/// The method with which the HTMl source was generated.
	/// </summary>
	public HtmlSourceMethod HtmlSourceMethod => HtmlSource.SourceMethod;
	IEnumerable<ITranspiledSource> ITranspilationResult.Sources =>
		HtmlSourceMethod == HtmlSourceMethod.Combined
		? new ITranspiledSource[] { HtmlSource, JavascriptSource! }
		: new[] { HtmlSource };
	/// <summary>
	/// The HTML source.
	/// </summary>
	public HtmlSource HtmlSource { get; }
	/// <summary>
	/// The Javascript source.
	/// </summary>
	/// <remarks>
	/// Is only set if the source method is <see cref="HtmlSourceMethod.Separate"/>.
	/// </remarks>
	public JavascriptSource? JavascriptSource => HtmlSource.Javascript;



	internal WebResult(ISemanticContext semanticContext, string htmlSource) {
		SemanticContext = semanticContext;
		HtmlSource = new(htmlSource);
	}
	internal WebResult(ISemanticContext semanticContext, string htmlSource, string javascriptSource) {
		SemanticContext = semanticContext;
		HtmlSource = new(htmlSource, javascriptSource);
	}

}
