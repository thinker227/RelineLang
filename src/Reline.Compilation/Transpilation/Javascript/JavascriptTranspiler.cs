using Reline.Compilation.Symbols;

namespace Reline.Compilation.Transpilation.Javascript;

/// <summary>
/// Transpiles a Reline semantic model into executable ES6 Javascript.
/// </summary>
public sealed class JavascriptTranspiler {

	private ISemanticContext Context { get; }
	private JavascriptTranspilationOptions Options { get; }



	private JavascriptTranspiler(ISemanticContext context, JavascriptTranspilationOptions options) {
		Context = context;
		Options = options;
	}



	/// <summary>
	/// Transpiles an <see cref="ISemanticContext"/> into Javascript.
	/// </summary>
	/// <param name="semanticContext">The semantic model to transpile.</param>
	/// <param name="options">The options to use when transpiling.</param>
	/// <returns>An <see cref="IJavscriptTranspilationResult"/> containing
	/// generated source files.</returns>
	public static IJavscriptTranspilationResult Transpile(ISemanticContext semanticContext, JavascriptTranspilationOptions options = default) {
		JavascriptTranspiler transpiler = new(semanticContext, options);
		return new NodeResult(semanticContext, @"console.log(""uwu"");");
	}

}
