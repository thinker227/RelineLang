namespace Reline.Compilation.Binding;

/// <summary>
/// The type of a unary operator.
/// </summary>
public enum UnaryOperatorType {
	/// <summary>
	/// Unary <c>+</c> operator.
	/// </summary>
	Identity,
	/// <summary>
	/// Unary <c>-</c> operator.
	/// </summary>
	Negation,
}

/// <summary>
/// The type of a binary operator.
/// </summary>
public enum BinaryOperatorType {
	/// <summary>
	/// Binary <c>+</c> operator.
	/// </summary>
	Addition,
	/// <summary>
	/// Binary <c>-</c> operator.
	/// </summary>
	Subtraction,
	/// <summary>
	/// Binary <c>*</c> operator.
	/// </summary>
	Multiplication,
	/// <summary>
	/// Binary <c>/</c> operator.
	/// </summary>
	Division,
	/// <summary>
	/// Binary <c>%</c> operator.
	/// </summary>
	Modulo,
	/// <summary>
	/// Binary <c>&lt;</c> operator.
	/// </summary>
	Concatenation,
	/// <summary>
	/// Binary <c>..</c> operator.
	/// </summary>
	Range
}

/// <summary>
/// The type of a keyword expression.
/// </summary>
public enum KeywordExpressionType {
	/// <summary>
	/// <c>here</c> keyword.
	/// </summary>
	Here,
	/// <summary>
	/// <c>start</c> keyword.
	/// </summary>
	Start,
	/// <summary>
	/// <c>end</c> keyword.
	/// </summary>
	End
}
