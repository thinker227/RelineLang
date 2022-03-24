using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Binding;

internal partial class Binder {

	/// <summary>
	/// Partially binds a <see cref="ProgramSyntax"/>
	/// into a <see cref="ProgramSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="ProgramSymbol.StartLine"/>
	/// and <see cref="ProgramSymbol.EndLine"/>.
	/// </remarks>
	private ProgramSymbol BindProgramPartialFromTree() {
		var syntax = SyntaxTree.Root;
		var symbol = Factory.CreateProgram(syntax);
		BindLinesPartial(symbol);
		return symbol;
	}
	/// <summary>
	/// Fully binds the lines of a <see cref="ProgramSymbol"/>.
	/// </summary>
	private void BindProgram(ProgramSymbol program) {
		// The lines of the program have already been partially bound in BindLinesPartial
		foreach (var l in program.Lines)
			BindLine(l);
	}
	/// <summary>
	/// Partially binds the lines of a <see cref="ProgramSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="LineSymbol.LineNumber"/> of each line.
	/// </remarks>
	private void BindLinesPartial(ProgramSymbol program) {
		var lines = ((ProgramSyntax)program.Syntax!).Lines;
		foreach (var lineSyntax in lines) {
			var line = BindLinePartial(lineSyntax);
			program.Lines[line.LineNumber - 1] = line;
		}
	}
	/// <summary>
	/// Partially binds a <see cref="LineSyntax"/> into a <see cref="LineSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="LineSymbol.LineNumber"/>.
	/// </remarks>
	private LineSymbol BindLinePartial(LineSyntax syntax) {
		var symbol = GetSymbol<LineSymbol>(syntax);
		symbol.LineNumber = syntax.LineNumber;
		return symbol;
	}
	/// <summary>
	/// Fully binds a <see cref="LineSymbol"/>.
	/// </summary>
	private void BindLine(LineSymbol symbol) {
		var statement = ((LineSyntax)symbol.Syntax!).Statement;
		if (statement is not null)
			symbol.Statement = BindStatement(statement);
	}

}
