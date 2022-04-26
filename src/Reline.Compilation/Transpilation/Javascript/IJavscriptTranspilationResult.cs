namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// The result of a Javascript transpilation operation.
/// </summary>
public interface IJavscriptTranspilationResult : ITranspilationResult {

	/// <summary>
	/// The <see cref="JavascriptTarget"/> targeted by the transpilation.
	/// </summary>
	JavascriptTarget Target { get; }

}
