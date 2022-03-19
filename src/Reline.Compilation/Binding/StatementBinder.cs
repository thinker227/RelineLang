﻿using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

internal partial class Binder {

	/// <summary>
	/// Binds an <see cref="IStatementSyntax"/> into an <see cref="IStatementSymbol"/>.
	/// </summary>
	private IStatementSymbol BindStatement(IStatementSyntax syntax) => syntax switch {
		ExpressionStatementSyntax s => BindExpressionStatement(s),
		AssignmentStatementSyntax s => BindAssignmentStatement(s),
		IManipulationStatementSyntax s => BindManipulationStatement(s),
		FunctionDeclarationStatementSyntax s => BindFunctionDeclarationStatement(s),
		ReturnStatementSyntax s => BindReturnStatement(s),

		_ => throw new NotSupportedException($"Statement type {syntax.GetType().Name} is not supported.")
	};

	/// <summary>
	/// Binds an <see cref="ExpressionStatementSyntax"/>
	/// into an <see cref="ExpressionStatementSymbol"/>.
	/// </summary>
	private ExpressionStatementSymbol BindExpressionStatement(ExpressionStatementSyntax syntax) {
		var expression = BindExpression(syntax.Expression);
		var symbol = GetSymbol<ExpressionStatementSymbol>(syntax);
		symbol.Expression = expression;
		return symbol;
	}
	/// <summary>
	/// Binds an <see cref="AssignmentStatementSyntax"/>
	/// into an <see cref="AssignmentStatementSymbol"/>.
	/// </summary>
	private AssignmentStatementSymbol BindAssignmentStatement(AssignmentStatementSyntax syntax) {
		var initializer = BindExpression(syntax.Initializer);

		string identifier = syntax.Identifier.Text;
		var variable = VariableBinder.GetSymbol(identifier);

		// If the variable is null and is missing then
		// it has already been reported as an error
		var symbol = GetSymbol<AssignmentStatementSymbol>(syntax);
		symbol.Variable = variable;
		symbol.Initializer = initializer;
		variable?.References.Add(symbol);
		return symbol;
	}
	/// <summary>
	/// Binds an <see cref="IManipulationStatementSyntax"/>
	/// into an <see cref="IStatementSymbol"/>.
	/// </summary>
	private IStatementSymbol BindManipulationStatement(IManipulationStatementSyntax syntax) {
		var source = BindExpression(syntax.Source);
		var target = BindExpression(syntax.Target);

		switch (syntax) {
			case MoveStatementSyntax:
				var move = GetSymbol<MoveStatementSymbol>(syntax);
				move.Source = source;
				move.Target = target;
				return move;
			case SwapStatementSyntax:
				var swap = GetSymbol<SwapStatementSymbol>(syntax);
				swap.Source = source;
				swap.Target = target;
				return swap;
			case CopyStatementSyntax:
				var copy = GetSymbol<CopyStatementSymbol>(syntax);
				copy.Source = source;
				copy.Target = target;
				return copy;
		}

		throw new NotSupportedException("Manipulation statement type {syntax.GetType().Name} is not supported.");
	}
	/// <summary>
	/// Binds a <see cref="FunctionDeclarationStatementSyntax"/> into a <see cref="FunctionDeclarationStatementSymbol"/>.
	/// </summary>
	private FunctionDeclarationStatementSymbol BindFunctionDeclarationStatement(FunctionDeclarationStatementSyntax syntax) =>
		GetSymbol<FunctionDeclarationStatementSymbol>(syntax);
	/// <summary>
	/// Binds a <see cref="ReturnStatementSyntax"/>
	/// into a <see cref="ReturnStatementSymbol"/>.
	/// </summary>
	private ReturnStatementSymbol BindReturnStatement(ReturnStatementSyntax syntax) {
		var expression = BindExpression(syntax.Expression);

		int line = SyntaxTree.GetAncestor<LineSyntax>(syntax)!.LineNumber;
		var function = FunctionBinder.GetDefinedFunctions()
			.FirstOrDefault(f => f.Range.Contains(line));
		if (function is null) {
			this.AddDiagnostic(syntax, CompilerDiagnostics.returnOutsideFunction);
		}

		var symbol = GetSymbol<ReturnStatementSymbol>(syntax);
		symbol.Expression = expression;
		symbol.Function = function;
		return symbol;
	}

}
