namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// Javascript tagrtes able to be transpiled to.
/// </summary>
public enum JavascriptTarget {
	/// <summary>
	/// Transpile to JS running on Node.js.
	/// </summary>
	Node,
	/// <summary>
	/// Transpile to JS and HTML running in a browser.
	/// </summary>
	Web
}
