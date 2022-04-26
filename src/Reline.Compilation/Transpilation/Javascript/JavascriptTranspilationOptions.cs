namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// The options when transpiling to Javascript.
/// </summary>
public readonly record struct JavascriptTranspilationOptions {

	/// <summary>
	/// The environment to target.
	/// </summary>
	public JavascriptTarget Target { get; init; } = JavascriptTarget.Node;
	/// <summary>
	/// The method with which to generate HTML source.
	/// </summary>
	/// <remarks>
	/// Only applies if the target is <see cref="JavascriptTarget.Web"/>.
	/// </remarks>
	public HtmlSourceMethod HtmlSourceMethod { get; init; } = HtmlSourceMethod.Separate;



	/// <summary>
	/// Initializes a new <see cref="JavascriptTranspilationOptions"/> instance.
	/// </summary>
	public JavascriptTranspilationOptions() { }

}
