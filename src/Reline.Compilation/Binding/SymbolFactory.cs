using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Binding;

/// <summary>
/// A factory for symbols.
/// </summary>
internal sealed class SymbolFactory {

	private readonly IBindingContext context;



	public SymbolFactory(IBindingContext context) {
		this.context = context;
	}



	/// <summary>
	/// Creates a new <see cref="ProgramSymbol"/>
	/// from a <see cref="ProgramSyntax"/>.
	/// </summary>
	/// <param name="syntax">The syntax to create the symbol from.</param>
	public ProgramSymbol CreateProgram(ProgramSyntax syntax) =>
		context.GetSymbol(syntax, s =>
			new ProgramSymbol(1, s.Lines.Length) { Syntax = syntax });

}
