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

	public LiteralValue VisitUnary(UnaryExpressionSymbol symbol) {
		var expression = symbol.Expression.Accept(this);

		throw new NotImplementedException();
	}
	public LiteralValue VisitBinary(BinaryExpressionSymbol symbol) {
		var l = symbol.Left.Accept(this);
		var r = symbol.Right.Accept(this);

		throw new NotImplementedException();
	}
	public LiteralValue VisitKeyword(KeywordExpressionSymbol symbol) {
		throw new NotImplementedException();
	}

	public LiteralValue VisitLiteral(LiteralExpressionSymbol symbol) =>
		symbol.Literal.Type != LiteralType.None ?
		symbol.Literal : throw new CompilationException("Cannot evaluate invalid literal.");
	// Could work with native functions with compile-time implementation
	public LiteralValue VisitFunctionInvocation(FunctionInvocationExpressionSymbol symbol) =>
		throw new CompilationException("Cannot evaluate function invocation.");
	// Could work if constant variables are implemented
	public LiteralValue VisitVariable(IdentifierExpressionSymbol symbol) =>
		throw new CompilationException("Cannot evaluate variable.");

}
