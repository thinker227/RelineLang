using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Evaluates constant expressions.
/// </summary>
internal sealed class ConstantExpressionEvaluator : IExpressionVisitor<LiteralValue> {

	private readonly Binder binder;



	public ConstantExpressionEvaluator(Binder binder) {
		this.binder = binder;
	}



	/// <summary>
	/// Evaluates a constant <see cref="IExpressionSymbol"/>.
	/// </summary>
	/// <param name="symbol">The symbol to evaluate.</param>
	/// <returns>The evaluated value of <paramref name="symbol"/>.</returns>
	/// <exception cref="CompilationException">
	/// The expression is not constant or the expression is invalid.
	/// </exception>
	public LiteralValue EvaluateExpression(IExpressionSymbol symbol) {
		if (!symbol.IsConstant) throw new CompilationException("Cannot evaluate non-constant expression.");
		return symbol.Accept(this);
	}

	public LiteralValue VisitUnaryPlus(UnaryPlusExpressionSymbol symbol) => symbol.Expression.Accept(this).Switch(
		i => new LiteralValue(+i),
		s => throw new CompilationException("Cannot apply unary plus to string."),
		r => throw new CompilationException("Cannot apply unary plus to range.")
	);
	public LiteralValue VisitUnaryNegation(UnaryNegationExpressionSymbol symbol) => symbol.Expression.Accept(this).Switch(
		i => new LiteralValue(-i),
		s => throw new CompilationException("Cannot apply unary negation to string."),
		r => throw new CompilationException("Cannot apply unary negation to range.")
	);
	// Maybe reconsider what "line pointers" actually are
	public LiteralValue VisitUnaryLinePointer(UnaryLinePointerExpressionSymbol symbol) =>
		throw new NotImplementedException("Implement range type.");
	public LiteralValue VisitUnaryFunctionPointer(UnaryFunctionPointerExpressionSymbol symbol) {
		var function = binder.functionBinder.GetSymbol(symbol.Identifier);
		if (function is null)
			throw new CompilationException($"Could not find function '{symbol.Identifier}'.");
		return function.BodyExpression.Accept(this);
	}

	// Copy + paste galore
	public LiteralValue VisitBinaryAddition(BinaryAdditionExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);
		
		if (l.TryGetAs(out int li) && r.TryGetAs(out int ri))
			return li + ri;

		throw new CompilationException("Cannot add non-number types.");
	}
	public LiteralValue VisitBinarySubtraction(BinarySubtractionExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);

		if (l.TryGetAs(out int li) && r.TryGetAs(out int ri))
			return li - ri;

		throw new CompilationException("Cannot subtract non-number types.");
	}
	public LiteralValue VisitBinaryMultiplication(BinaryMultiplicationExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);
		
		if (l.TryGetAs(out int li) && r.TryGetAs(out int ri))
			return li * ri;

		throw new CompilationException("Cannot multiply non-number types.");
	}
	public LiteralValue VisitBinaryDivision(BinaryDivisionExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);

		if (l.TryGetAs(out int li) && r.TryGetAs(out int ri))
			return li / ri;

		throw new CompilationException("Cannot divide non-number types.");
	}
	public LiteralValue VisitBinaryModulo(BinaryModuloExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);

		if (l.TryGetAs(out int li) && r.TryGetAs(out int ri))
			return li % ri;

		throw new CompilationException("Cannot modulo non-number types.");
	}
	public LiteralValue VisitBinaryConcatenation(BinaryConcatenationExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);

		if (l.TryGetAs(out string? li) && r.TryGetAs(out string? ri))
			return string.Concat(li, ri);

		throw new CompilationException("Cannot concatenate non-string types.");
	}

	// Implement source file information
	public LiteralValue VisitStart(StartExpressionSymbol symbol) =>
		throw new NotImplementedException("Implement source file information.");
	public LiteralValue VisitEnd(EndExpressionSymbol symbol) =>
		throw new NotImplementedException("Implement source file information.");
	// Implement symbol line or parent
	public LiteralValue VisitHere(HereExpressionSymbol symbol) =>
		throw new NotImplementedException("Implement symbol line or parent.");

	public LiteralValue VisitLiteral(LiteralExpressionSymbol symbol) =>
		symbol.Literal.Type != LiteralType.None ?
		symbol.Literal : throw new CompilationException("Cannot evaluate invalid literal.");
	public LiteralValue VisitGrouping(GroupingExpressionSymbol symbol) =>
		symbol.Expression.Accept(this);
	public LiteralValue VisitRange(RangeExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);

		if (l.TryGetAs(out int li) && r.TryGetAs(out int ri))
			return new RangeLiteral(li, ri);

		throw new CompilationException("Cannot create range between non-number types.");
	}
	// Could work with native functions with compile-time implementation
	public LiteralValue VisitFunctionInvocation(FunctionInvocationExpressionSymbol symbol) =>
		throw new CompilationException("Cannot evaluate function invocation.");
	// Could work if constant variables are implemented
	public LiteralValue VisitVariable(VariableExpressionSymbol symbol) =>
		throw new CompilationException("Cannot evaluate variable.");

}
