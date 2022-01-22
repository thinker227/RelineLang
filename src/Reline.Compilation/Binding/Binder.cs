using Reline.Compilation.Diagnostics;
using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds syntax nodes into symbols.
/// </summary>
public sealed partial class Binder {

	private readonly SyntaxTree tree;
	private readonly SyntaxSymbolBinder syntaxSymbolBinder;
	private readonly LabelBinder labelBinder;
	private readonly VariableBinder variableBinder;
	private readonly FunctionBinder functionBinder;



	private Binder(SyntaxTree tree) {
		this.tree = tree;
		syntaxSymbolBinder = new();
		labelBinder = new();
		variableBinder = new();
		functionBinder = new();
	}



	/// <summary>
	/// Binds a <see cref="SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	/// <param name="tree">The syntax tree to bind.</param>
	/// <returns>An <see cref="IOperationResult{T}"/>
	/// containing the bound <see cref="SymbolTree"/>.</returns>
	public static IOperationResult<SymbolTree> BindTree(SyntaxTree tree) {
		Binder symbolCompiler = new(tree);
		var result = symbolCompiler.BindTree();
		return new SymbolCompilationResult(ImmutableArray<Diagnostic>.Empty, result);
	}
	/// <summary>
	/// Binds a <see cref="SyntaxTree"/> into a <see cref="SymbolTree"/>.
	/// </summary>
	private SymbolTree BindTree() {
		BindLabelsFromTree();
		BindVariablesFromAssignments();
		BindFunctionsFromTree();
		var program = BindProgram(tree.Root);
		return new(program);
	}

	private void BindLabelsFromTree() {
		var labels = tree.GetLabels();
		foreach (var label in labels) {
			var symbol = CreateSymbol<LabelSymbol>(label);
			symbol.Identifier = label.Identifier.Text;
			labelBinder.RegisterSymbol(symbol);
		}
	}
	private void BindVariablesFromAssignments() {
		var assignments = tree.GetStatementsOfType<AssignmentStatementSyntax>();
		foreach (var assignment in assignments) {
			VariableSymbol symbol = new() {
				Identifier = assignment.Identifier.Text
			};
			variableBinder.RegisterSymbol(symbol);
		}
	}
	private void BindFunctionsFromTree() {
		var functions = tree.GetStatementsOfType<FunctionDeclarationStatementSyntax>();
		foreach (var function in functions) {
			var symbol = CreateSymbol<FunctionSymbol>(function);
			symbol.Identifier = function.Identifier.Text;
			functionBinder.RegisterSymbol(symbol);
		}
	}

	/// <summary>
	/// Binds a <see cref="ProgramSyntax"/>
	/// into a <see cref="ProgramSymbol"/>.
	/// </summary>
	/// <param name="syntax"></param>
	/// <returns></returns>
	private ProgramSymbol BindProgram(ProgramSyntax syntax) {
		var symbol = CreateSymbol<ProgramSymbol>(syntax);

		foreach (var l in syntax.Lines) {
			symbol.Lines.Add(BindLine(l));
		}

		return symbol;
	}
	/// <summary>
	/// Binds a <see cref="LineSyntax"/>
	/// into a <see cref="LineSymbol"/>.
	/// </summary>
	/// <param name="syntax"></param>
	/// <returns></returns>
	private LineSymbol BindLine(LineSyntax syntax) {
		var symbol = CreateSymbol<LineSymbol>(syntax);

		if (syntax.Label is not null && !syntax.Label.Identifier.IsMissing) {
			var identifier = syntax.Label.Identifier.Text;
			var label = CreateSymbol<LabelSymbol>(syntax.Label);
			label.Identifier = identifier;
			label.Line = symbol;
			symbol.Label = label;
		}

		if (syntax.Statement is not null) {
			BindLineStatement(symbol, syntax.Statement);
		}

		return symbol;
	}

	/// <summary>
	/// If <paramref name="syntax"/> is not present in <see cref="syntaxSymbolBinder"/>,
	/// creates a symbol with <see cref="ISymbol.Syntax"/> set to <paramref name="syntax"/>
	/// and the symbol registered in <see cref="syntaxSymbolBinder"/>
	/// if <paramref name="syntax"/> is not <see langword="null"/>.
	/// Otherwise, returns the <see cref="ISymbol"/> in <see cref="syntaxSymbolBinder"/>.
	/// </summary>
	private TSymbol CreateSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new() {
		if (syntaxSymbolBinder.TryGetSymbol(syntax, out var bound))
			return (TSymbol)bound;

		TSymbol symbol = new() { Syntax = syntax };
		syntaxSymbolBinder.Bind(syntax, symbol);
		return symbol;
	}

}
