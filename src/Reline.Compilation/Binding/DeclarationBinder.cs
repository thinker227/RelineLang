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
		foreach (var lineSyntax in Tree.Root.Lines) BindLabelFromLine(lineSyntax);
	}
	/// <summary>
	/// Binds the <see cref="LabelSyntax"/> of a <see cref="LineSyntax"/>
	/// into a <see cref="LabelSymbol"/>.
	/// </summary>
	private void BindLabelFromLine(LineSyntax lineSyntax) {
		var labelSyntax = lineSyntax.Label;
		if (labelSyntax is null) return;

		var label = CreateSymbol<LabelSymbol>(labelSyntax);
		var line = BindLinePartialFromLabel(lineSyntax, label);
		label.Identifier = labelSyntax.Identifier.Text;
		label.Line = line;
		line.Label = label;

		LabelBinder.RegisterSymbol(label);
	}
	/// <summary>
	/// Partially binds a <see cref="LabelSyntax"/> into a <see cref="LineSymbol"/>
	/// from a <see cref="LabelSymbol"/>.
	/// </summary>
	/// <remarks>
	/// Only binds <see cref="LineSymbol.LineNumber"/> and <see cref="LineSymbol.Label"/>.
	/// </remarks>
	private LineSymbol BindLinePartialFromLabel(LineSyntax syntax, LabelSymbol label) {
		var symbol = BindLinePartial(syntax);
		symbol.Label = label;
		return symbol;
	}
	/// <summary>
	/// Binds all variables from assignments.
	/// </summary>
	private void BindVariablesFromAssignments() {
		var assignments = Tree.GetStatementsOfType<AssignmentStatementSyntax>();
		foreach (var assignment in assignments) {
			VariableSymbol symbol = new() {
				Identifier = assignment.Identifier.Text
			};
			VariableBinder.RegisterSymbol(symbol);
		}
	}
	/// <summary>
	/// Binds all functions and parameters from the tree.
	/// </summary>
	private void BindFunctionsFromTree() {
		var functions = Tree.GetStatementsOfType<FunctionDeclarationStatementSyntax>();
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
		FunctionBinder.RegisterSymbol(symbol);

		if (symbol.Range.GetValueType() != VType.Range || function.ParameterList is null) return;

		foreach (var p in function.ParameterList.Parameters) {
			var range = ExpressionEvaluator.EvaluateExpression(symbol.Range).GetAs<RangeLiteral>();
			ParameterSymbol parameter = new() {
				Identifier = p.Text,
				Range = range,
				Function = symbol
			};
			VariableBinder.RegisterSymbol(parameter);
		}
	}

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
