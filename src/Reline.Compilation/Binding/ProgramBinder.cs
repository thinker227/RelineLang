namespace Reline.Compilation.Binding;

partial class Binder {

	/// <summary>
	/// Binds a <see cref="ProgramSyntax"/>
	/// into a <see cref="ProgramSymbol"/>.
	/// </summary>
	private ProgramSymbol BindProgram(ProgramSyntax syntax) {
		var symbol = CreateSymbol<ProgramSymbol>(syntax);
		foreach (var l in syntax.Lines)
			symbol.Lines.Add(BindLine(l));
		return symbol;
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
