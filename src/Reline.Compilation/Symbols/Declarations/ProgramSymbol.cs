﻿namespace Reline.Compilation.Symbols;

public sealed class ProgramSymbol : SymbolNode {

	public LineList Lines { get; } = new();
	public int StartLine { get; set; }
	public int EndLine { get; set; }
	public IList<LabelSymbol> Labels { get; } = new List<LabelSymbol>();
	public IList<IVariableSymbol> Variables { get; } = new List<IVariableSymbol>();
	public IList<FunctionSymbol> Functions { get; } = new List<FunctionSymbol>();

}
