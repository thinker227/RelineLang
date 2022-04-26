using Reline.Compilation.Symbols;

namespace Reline.Compilation.Transpilation;

/// <summary>
/// The result of a transpilation operation.
/// </summary>
public interface ITranspilationResult {

	/// <summary>
	/// The <see cref="ISemanticContext"/> which was transpiled.
	/// </summary>
	ISemanticContext SemanticContext { get; }
	/// <summary>
	/// The transpiled sources.
	/// </summary>
	IEnumerable<ITranspiledSource> Sources { get; }

}
