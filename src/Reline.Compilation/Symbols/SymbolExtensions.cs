using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation.Symbols;

public static class SymbolExtensions {

	private static ValueType Union(BinaryExpressionSymbol binary) =>
		Union(binary.Left.GetValueType(), binary.Right.GetValueType());
	private static ValueType Union(ValueType left, ValueType right) =>
		left == right ? left : ValueType.None;
	/// <summary>
	/// Gets the <see cref="ValueType"/> of an <see cref="IExpressionSymbol"/>.
	/// </summary>
	/// <param name="expression">The expression to get the type of.</param>
	/// <returns>The <see cref="ValueType"/> of <paramref name="expression"/>.</returns>
	public static ValueType GetValueType(this IExpressionSymbol expression) => expression switch {
		RangeExpressionSymbol => ValueType.Range,
		LiteralExpressionSymbol s => (ValueType)s.Literal.Type,
		GroupingExpressionSymbol s => s.Expression.GetValueType(),
		FunctionInvocationExpressionSymbol or VariableExpressionSymbol => ValueType.Mixed,

		UnaryPlusExpressionSymbol s => s.Expression.GetValueType(),
		UnaryNegationExpressionSymbol s => s.Expression.GetValueType(),
		UnaryFunctionPointerExpressionSymbol => ValueType.Range,
		UnaryLinePointerExpressionSymbol => ValueType.Range,

		BinaryExpressionSymbol s => Union(s),

		StartExpressionSymbol or
		EndExpressionSymbol or
		HereExpressionSymbol => ValueType.Range,

		_ => ValueType.None
	};

}
