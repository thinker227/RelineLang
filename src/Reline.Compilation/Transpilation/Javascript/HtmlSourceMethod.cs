namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// The method to generate HTML source with.
/// </summary>
public enum HtmlSourceMethod {
	/// <summary>
	/// Generate separate sources for JS and HTML.
	/// </summary>
	Separate,
	/// <summary>
	/// Generate a single HTML source with included JS.
	/// </summary>
	Combined
}
