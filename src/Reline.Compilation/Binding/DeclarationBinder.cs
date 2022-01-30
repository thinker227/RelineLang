using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using VType = Reline.Compilation.Symbols.ValueType;

namespace Reline.Compilation.Binding;

partial class Binder {

	/// <summary>
	/// Binds all labels from the tree.
	/// </summary>
	/// <remarks>
	/// This does not resolve <see cref="LabelSymbol.Line"/>.
	/// </remarks>
	private void BindLabelsFromTree() {
		var labels = tree.GetLabels();
		foreach (var label in labels) {
			var symbol = CreateSymbol<LabelSymbol>(label);
			symbol.Identifier = label.Identifier.Text;
			labelBinder.RegisterSymbol(symbol);
		}
	}
	/// <summary>
	/// Binds all variables from assignments.
	/// </summary>
	private void BindVariablesFromAssignments() {
		var assignments = tree.GetStatementsOfType<AssignmentStatementSyntax>();
		foreach (var assignment in assignments) {
			VariableSymbol symbol = new() {
				Identifier = assignment.Identifier.Text
			};
			variableBinder.RegisterSymbol(symbol);
		}
	}
	/// <summary>
	/// Binds all functions and parameters from the tree.
	/// </summary>
	/// <remarks>
	/// This does not resolve
	/// <see cref="FunctionSymbol.Parameters"/>,
	/// <see cref="FunctionSymbol.Range"/> or
	/// <see cref="FunctionSymbol.Body"/>.
	/// </remarks>
	private void BindFunctionsFromTree() {
		var functions = tree.GetStatementsOfType<FunctionDeclarationStatementSyntax>();
		foreach (var function in functions) BindFunction(function);
	}
	/// <summary>
	/// Binds a function and its parameters.
	/// </summary>
	private void BindFunction(FunctionDeclarationStatementSyntax function) {
		var symbol = CreateSymbol<FunctionSymbol>(function);
		symbol.Identifier = function.Identifier.Text;
		// Bind body expression immediately in order to bind parameters to the proper range
		symbol.Range = BindExpression(function.Body, ExpressionBindingFlags.ConstantsLabels, VType.Range);
		functionBinder.RegisterSymbol(symbol);

		if (symbol.Range.GetValueType() != VType.Range || function.ParameterList is null) return;

		foreach (var p in function.ParameterList.Parameters) {
			var range = expressionEvaluator.EvaluateExpression(symbol.Range).GetAs<RangeLiteral>();
			ParameterSymbol parameter = new() {
				Identifier = p.Text,
				Range = range,
				Function = symbol
			};
			variableBinder.RegisterSymbol(parameter);
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
			symbol.Label = BindLabel(syntax.Label, symbol);
		}

		if (syntax.Statement is not null) {
			symbol.Statement = BindStatement(syntax.Statement);
		}

		return symbol;
	}
	/// <summary>
	/// Binds a <see cref="LabelSyntax"/> into a <see cref="LabelSymbol"/>.
	/// </summary>
	private LabelSymbol BindLabel(LabelSyntax syntax, LineSymbol line) {
		var symbol = CreateSymbol<LabelSymbol>(syntax);
		var identifier = syntax.Identifier.Text;
		symbol.Identifier = identifier;
		symbol.Line = line;
		return symbol;
	}

}
