namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// An HTML source.
/// </summary>
public sealed class HtmlSource : ITranspiledSource {

	public string Source { get; }
	public ISourceType Type => HtmlSourceType.Instance;
	/// <summary>
	/// The method with which the HTML source was generated.
	/// </summary>
	public HtmlSourceMethod SourceMethod { get; }
	/// <summary>
	/// The linked Javascript source.
	/// </summary>
	/// <remarks>
	/// Is only set if the source method is <see cref="HtmlSourceMethod.Separate"/>.
	/// </remarks>
	public JavascriptSource? Javascript { get; }



	internal HtmlSource(string source) {
		Source = source;
		SourceMethod = HtmlSourceMethod.Combined;
		Javascript = null;
	}
	internal HtmlSource(string source, string javascriptSource) {
		Source = source;
		SourceMethod = HtmlSourceMethod.Separate;
		Javascript = new(javascriptSource);
	}

}

/// <summary>
/// The HTML source type.
/// </summary>
public sealed class HtmlSourceType : ISourceType {

	/// <summary>
	/// A singleton instance.
	/// </summary>
	public static HtmlSourceType Instance { get; } = new();

	public string Name => "HTML";
	public string Extension => ".html";

	private HtmlSourceType() { }

}
