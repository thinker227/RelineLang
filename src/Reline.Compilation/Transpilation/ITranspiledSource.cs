namespace Reline.Compilation.Transpilation;

/// <summary>
/// A transpiled source.
/// </summary>
public interface ITranspiledSource {

	/// <summary>
	/// The source text.
	/// </summary>
	string Source { get; }
	/// <summary>
	/// The type of the source.
	/// </summary>
	ISourceType Type { get; }

}
