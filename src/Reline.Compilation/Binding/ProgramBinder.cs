using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Binding;

partial class Binder {

	/// <summary>
	/// Partially binds <see cref="Parsing.SyntaxTree.Root"/>
	/// into a <see cref="ProgramSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="ProgramSymbol.StartLine"/>
	/// and <see cref="ProgramSymbol.EndLine"/>.
	/// </remarks>
	private ProgramSymbol BindProgramPartial() {
		var syntax = SyntaxTree.Root;
		var symbol = CreateSymbol<ProgramSymbol>(syntax);
		symbol.StartLine = 1;
		symbol.EndLine = syntax.Lines.Length;
		return symbol;
	}
	/// <summary>
	/// Binds <see cref="Parsing.SyntaxTree.Root"/>
	/// into <see cref="ProgramRoot"/>.
	/// </summary>
	private void BindProgram() {
		var symbol = programRoot;
		foreach (var l in SyntaxTree.Root.Lines)
			symbol.Lines.Add(BindLine(l));
	}
	/// <summary>
	/// Partially binds a <see cref="LineSyntax"/> into a <see cref="LineSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="LineSymbol.LineNumber"/>.
	/// </remarks>
	private LineSymbol BindLinePartial(LineSyntax syntax) {
		var symbol = CreateSymbol<LineSymbol>(syntax);
		symbol.LineNumber = syntax.LineNumber;
		return symbol;
	}
	/// <summary>
	/// Binds a <see cref="LineSyntax"/>
	/// into a <see cref="LineSymbol"/>.
	/// </summary>
	private LineSymbol BindLine(LineSyntax syntax) {
		var symbol = BindLinePartial(syntax);
		CurrentLine = symbol;
		if (syntax.Statement is not null)
			symbol.Statement = BindStatement(syntax.Statement);
		return symbol;
	}

}
