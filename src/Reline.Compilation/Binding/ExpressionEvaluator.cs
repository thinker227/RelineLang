using Reline.Compilation.Diagnostics;
using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Binding;

/// <summary>
/// Evaluates compile-time expressions.
/// </summary>
internal sealed class ExpressionEvaluator : IExpressionVisitor<BoundValue> {

	private readonly ISemanticContext context;
	private readonly IDiagnosticContext? diagnostics;



	internal ExpressionEvaluator(ISemanticContext context, IDiagnosticContext? diagnostics) {
		this.context = context;
		this.diagnostics = diagnostics;
	}
	public ExpressionEvaluator(ISemanticContext context) : this(context, null) { }



	/// <summary>
	/// Evaluates a compile-time <see cref="IExpressionSymbol"/> and
	/// reports diagnostics about type errors and non-constant subexpressions.
	/// </summary>
	/// <param name="symbol">The symbol to evaluate.</param>
	/// <returns>The evaluated value of <paramref name="symbol"/>.</returns>
	public BoundValue EvaluateExpression(IExpressionSymbol symbol) =>
		symbol.Accept(this);

	public BoundValue VisitUnary(UnaryExpressionSymbol symbol) {
		var expression = symbol.Expression.Accept(this);

		if (expression.Type == BoundValueType.None) return new();

		switch ((symbol.OperatorType, expression.Type)) {
			case (UnaryOperatorType.Identity, BoundValueType.Number):
				return +expression.GetAs<int>();
			case (UnaryOperatorType.Negation, BoundValueType.Number):
				return -expression.GetAs<int>();
		}

		AddDiagnostic(symbol, CompilerDiagnostics.unaryOperatorTypeError, symbol.OperatorType, expression.Type);
		return new();
	}
	public BoundValue VisitBinary(BinaryExpressionSymbol symbol) {
		var left = symbol.Left.Accept(this);
		var right = symbol.Right.Accept(this);

		if (left.Type == BoundValueType.None || right.Type == BoundValueType.None) return new();

		switch ((symbol.OperatorType, left.Type, right.Type)) {
			case (BinaryOperatorType.Addition, BoundValueType.Number, BoundValueType.Number):
				return left.GetAs<int>() + right.GetAs<int>();
			case (BinaryOperatorType.Subtraction, BoundValueType.Number, BoundValueType.Number):
				return left.GetAs<int>() - right.GetAs<int>();
			case (BinaryOperatorType.Multiplication, BoundValueType.Number, BoundValueType.Number):
				return left.GetAs<int>() * right.GetAs<int>();
			case (BinaryOperatorType.Division, BoundValueType.Number, BoundValueType.Number):
				int r = right.GetAs<int>();
				if (r == 0) {
					AddDiagnostic(symbol, CompilerDiagnostics.divisionByZero);
					return new();
				}
				return left.GetAs<int>() / r;
			case (BinaryOperatorType.Modulo, BoundValueType.Number, BoundValueType.Number):
				return left.GetAs<int>() % right.GetAs<int>();
			case (BinaryOperatorType.Concatenation, BoundValueType.String, BoundValueType.String):
				return string.Concat(left.GetAs<string>(), right.GetAs<string>());
			case (BinaryOperatorType.Range, BoundValueType.Number, BoundValueType.Number):
				return new RangeValue(left.GetAs<int>(), right.GetAs<int>());
		}

		AddDiagnostic(symbol, CompilerDiagnostics.binaryOperatorTypeError, symbol.OperatorType, left.Type, right.Type);
		return new();
	}
	public BoundValue VisitKeyword(KeywordExpressionSymbol symbol) => symbol.KeywordType switch {
		KeywordExpressionType.Here =>
			context.GetAncestor<LineSymbol>(symbol)!.LineNumber,
		KeywordExpressionType.Start =>
			context.Root.StartLine,
		KeywordExpressionType.End =>
			context.Root.EndLine,
		_ => new(),
	};
	public BoundValue VisitLiteral(LiteralExpressionSymbol symbol) =>
		symbol.Literal;
	public BoundValue VisitFunctionInvocation(FunctionInvocationExpressionSymbol symbol) {
		AddDiagnostic(symbol, CompilerDiagnostics.disallowedFunctionInvocations);
		return new();
	}
	public BoundValue VisitFunctionPointer(FunctionPointerExpressionSymbol symbol) =>
		// FunctionSymbol.Range may not be set at this stage
		symbol.Function.RangeExpression.Accept(this);
	public BoundValue VisitVariable(IdentifierExpressionSymbol symbol) {
		switch (symbol.Identifier) {
			case LabelSymbol label:
				return label.Line.LineNumber;
			case IVariableSymbol:
				AddDiagnostic(symbol, CompilerDiagnostics.disallowedNonConstantsOnlyLabels);
				return new();
			// Function symbols will have been reported an error
		}

		return new();
	}
	public BoundValue VisitBad(BadExpressionSymbol symbol) =>
		new();

	private void AddDiagnostic(ISymbol symbol, DiagnosticDescription description, params object?[] formatArgs) =>
		diagnostics?.AddDiagnostic(symbol, description, formatArgs);

}
