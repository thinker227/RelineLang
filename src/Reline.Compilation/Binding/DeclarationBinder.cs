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
		foreach (var lineSyntax in SyntaxTree.Root.Lines) BindLabelFromLine(lineSyntax);
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
		var assignments = SyntaxTree.GetStatementsOfType<AssignmentStatementSyntax>();
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
		var functions = SyntaxTree.GetStatementsOfType<FunctionDeclarationStatementSyntax>();
		foreach (var function in functions) BindFunction(function);
	}
	/// <summary>
	/// Binds a function and its parameters.
	/// </summary>
	private void BindFunction(FunctionDeclarationStatementSyntax syntax) {
		var declaration = CreateSymbol<FunctionDeclarationStatementSymbol>(syntax);
		FunctionSymbol function = new();
		function.Declaration = declaration;
		declaration.Function = function;
		function.Identifier = syntax.Identifier.Text;

		// Bind body expression immediately in order to bind parameters to the proper range
		function.RangeExpression = BindExpression(syntax.Body, ExpressionBindingFlags.ConstantsLabels);
		FunctionBinder.RegisterSymbol(function);
		var range = ExpressionEvaluator.EvaluateExpression(function.RangeExpression);
		if (range.Type != LiteralType.Range) {
			// Reporting a type error when the error is due to an error is a subexpression looks ugly
			if (range.Type != LiteralType.None)
				AddDiagnostic(function.RangeExpression, Diagnostics.DiagnosticLevel.Error, "Expected constant range.");
			return;
		}
		var rangeValue = range.GetAs<RangeLiteral>();
		function.Range = rangeValue;

		if (syntax.ParameterList is null) return;
		foreach (var p in syntax.ParameterList.Parameters) {
			ParameterSymbol parameter = new() {
				Identifier = p.Text,
				Range = rangeValue,
				Function = function
			};
			function.Parameters.Add(parameter);
			VariableBinder.RegisterSymbol(parameter);
		}
	}

}
