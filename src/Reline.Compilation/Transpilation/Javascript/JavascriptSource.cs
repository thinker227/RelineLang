namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// A Javascript source.
/// </summary>
public sealed class JavascriptSource : ITranspiledSource {

	public string Source { get; }
	public ISourceType Type => JavascriptSourceType.Instance;



	internal JavascriptSource(string source) {
		Source = source;
	}

}

/// <summary>
/// The Javascript source type.
/// </summary>
public sealed class JavascriptSourceType : ISourceType {

	/// <summary>
	/// A singleton instance.
	/// </summary>
	public static JavascriptSourceType Instance { get; } = new();

	public string Name => "JavaScript";
	public string Extension => ".js";

	private JavascriptSourceType() { }

}
