using Reline.Compilation.Diagnostics;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Evaluates compile-time expressions.
/// </summary>
internal sealed class ExpressionEvaluator : IExpressionVisitor<LiteralValue> {

	private readonly Binder binder;



	public ExpressionEvaluator(Binder binder) {
		this.binder = binder;
	}



	/// <summary>
	/// Evaluates a compile-time <see cref="IExpressionSymbol"/> and
	/// reports diagnostics about type errors and non-constant subexpressions.
	/// </summary>
	/// <param name="symbol">The symbol to evaluate.</param>
	/// <returns>The evaluated value of <paramref name="symbol"/>.</returns>
	public LiteralValue EvaluateExpression(IExpressionSymbol symbol) =>
		symbol.Accept(this);

	public LiteralValue VisitUnary(UnaryExpressionSymbol symbol) {
		var expression = symbol.Expression.Accept(this);

		if (expression.Type == LiteralType.None) return new();

		switch ((symbol.OperatorType, expression.Type)) {
			case (UnaryOperatorType.Identity, LiteralType.Number):
				return +expression.GetAs<int>();
			case (UnaryOperatorType.Negation, LiteralType.Number):
				return -expression.GetAs<int>();
		}

		AddDiagnostic(symbol, CompilerDiagnostics.unaryOperatorTypeError, symbol.OperatorType, expression.Type);
		return new();
	}
	public LiteralValue VisitBinary(BinaryExpressionSymbol symbol) {
		var left = symbol.Left.Accept(this);
		var right = symbol.Right.Accept(this);

		if (left.Type == LiteralType.None || right.Type == LiteralType.None) return new();

		switch ((symbol.OperatorType, left.Type, right.Type)) {
			case (BinaryOperatorType.Addition, LiteralType.Number, LiteralType.Number):
				return left.GetAs<int>() + right.GetAs<int>();
			case (BinaryOperatorType.Subtraction, LiteralType.Number, LiteralType.Number):
				return left.GetAs<int>() - right.GetAs<int>();
			case (BinaryOperatorType.Multiplication, LiteralType.Number, LiteralType.Number):
				return left.GetAs<int>() * right.GetAs<int>();
			case (BinaryOperatorType.Division, LiteralType.Number, LiteralType.Number):
				int r = right.GetAs<int>();
				if (r == 0) {
					AddDiagnostic(symbol, CompilerDiagnostics.divisionByZero);
					return new();
				}
				return left.GetAs<int>() / r;
			case (BinaryOperatorType.Modulo, LiteralType.Number, LiteralType.Number):
				return left.GetAs<int>() % right.GetAs<int>();
			case (BinaryOperatorType.Concatenation, LiteralType.String, LiteralType.String):
				return string.Concat(left.GetAs<string>(), right.GetAs<string>());
			case (BinaryOperatorType.Range, LiteralType.Number, LiteralType.Number):
				return new RangeLiteral(left.GetAs<int>(), right.GetAs<int>());
		}

		AddDiagnostic(symbol, CompilerDiagnostics.binaryOperatorTypeError, symbol.OperatorType, left.Type, right.Type);
		return new();
	}
	public LiteralValue VisitKeyword(KeywordExpressionSymbol symbol) => symbol.KeywordType switch {
		// This is not a good way to evaluate 'here' expressions
		// because the expression being evaluated may not always
		// be on the current line, ex. when evaluating expression
		// during analysis or refactoring when the binder is not
		// doing anything. For this, a proper method to get an
		// ancestor symbol of a specified type would be required.
		KeywordExpressionType.Here => binder.CurrentLine!.LineNumber,
		KeywordExpressionType.Start => binder.ProgramRoot.StartLine,
		KeywordExpressionType.End => binder.ProgramRoot.EndLine,
		_ => new(),
	};
	public LiteralValue VisitLiteral(LiteralExpressionSymbol symbol) =>
		symbol.Literal;
	public LiteralValue VisitFunctionInvocation(FunctionInvocationExpressionSymbol symbol) {
		AddDiagnostic(symbol, CompilerDiagnostics.disallowedFunctionInvocations);
		return new();
	}
	public LiteralValue VisitFunctionPointer(FunctionPointerExpressionSymbol symbol) =>
		// FunctionSymbol.Range may not be set at this stage
		symbol.Function.RangeExpression.Accept(this);
	public LiteralValue VisitVariable(IdentifierExpressionSymbol symbol) {
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

	private void AddDiagnostic(ISymbol symbol, DiagnosticDescription description, params object?[] formatArgs) =>
		binder.AddDiagnostic(symbol, description, formatArgs);

}
