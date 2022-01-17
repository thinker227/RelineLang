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
	private readonly VariableBinder variableBinder;



	private Binder(SyntaxTree tree) {
		this.tree = tree;
		variableBinder = new();
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
		var program = BindProgram(tree.Root);
		return new(program);
	}

	/// <summary>
	/// Binds a <see cref="ProgramSyntax"/>
	/// into a <see cref="ProgramSymbol"/>.
	/// </summary>
	/// <param name="syntax"></param>
	/// <returns></returns>
	private ProgramSymbol BindProgram(ProgramSyntax syntax) {
		ProgramSymbol symbol = new() {
			Syntax = syntax
		};

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
		LineSymbol symbol = new() {
			Syntax = syntax
		};

		if (syntax.Label is not null && !syntax.Label.Identifier.IsMissing) {
			var identifier = syntax.Label.Identifier.Text;
			LabelSymbol label = new() {
				Syntax = syntax.Label,
				Identifier = identifier,
				Line = symbol
			};
			symbol.Label = label;
		}

		if (syntax.Statement is not null) {
			BindLineStatement(symbol, syntax.Statement);
		}

		return symbol;
	}

}
