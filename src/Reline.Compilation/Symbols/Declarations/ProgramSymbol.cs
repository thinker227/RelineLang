namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a program.
/// </summary>
public sealed class ProgramSymbol : SymbolNode {

	/// <summary>
	/// The lines of the program.
	/// </summary>
	public LineList Lines { get; } = new();
	/// <summary>
	/// The line number of first line of the program.
	/// </summary>
	public int StartLine { get; set; }
	/// <summary>
	/// The line number of the last line of the program.
	/// </summary>
	public int EndLine { get; set; }
	/// <summary>
	/// The declared labels in the program.
	/// </summary>
	public IList<LabelSymbol> Labels { get; } = new List<LabelSymbol>();
	/// <summary>
	/// The declared variables in the program.
	/// </summary>
	public IList<IVariableSymbol> Variables { get; } = new List<IVariableSymbol>();
	/// <summary>
	/// The declared functions in the program.
	/// </summary>
	public IList<FunctionSymbol> Functions { get; } = new List<FunctionSymbol>();

}
