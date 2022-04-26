namespace Reline.Compilation.Transpilation;

/// <summary>
/// The type of a transpiled source.
/// </summary>
public interface ISourceType {

	/// <summary>
	/// The readable name of the source type.
	/// </summary>
	string Name { get; }
	/// <summary>
	/// The file extension of the source type.
	/// </summary>
	string Extension { get; }

}
