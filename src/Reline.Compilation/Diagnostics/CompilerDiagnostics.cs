namespace Reline.Compilation.Diagnostics;

internal static class CompilerDiagnostics {

	// Organize these better

	/// <summary>
	/// <c>RL0001</c>: Unexpected character '{0}' at position {1} in source
	/// </summary>
	public static readonly DiagnosticDescription unexpectedCharacter = new() {
		ErrorCode = "RL0001",
		Description = "Unexpected character '{0}' at position {1} in source",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0002</c>: Invalid expression term '{0}'
	/// </summary>
	public static readonly DiagnosticDescription invalidExpressionTerm = new() {
		ErrorCode = "RL0002",
		Description = "Invalid expression term '{0}'",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0003</c>: Only function invocation expressions may be used as expression statements
	/// </summary>
	public static readonly DiagnosticDescription invalidExpressionStatement = new() {
		ErrorCode = "RL0003",
		Description = "Only function invocation expressions may be used as expression statements",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0004</c>: Unexpected expression type
	/// </summary>
	public static readonly DiagnosticDescription unexpectedExpressionType = new() {
		ErrorCode = "RL0004",
		Description = "Unexpected expression type",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0005</c>: An identifier with the name '{0}' is already defined.
	/// </summary>
	public static readonly DiagnosticDescription identifierAlreadyDefined = new() {
		ErrorCode = "RL0005",
		Description = "An identifier with the name '{0}' is already defined",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0006</c>: Expected type '{0}' 
	/// </summary>
	public static readonly DiagnosticDescription expectedType = new() {
		ErrorCode = "RL0006",
		Description = "Expected type '{0}'",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0007</c>: Labels, variables, parameters and functions may not be used in this context
	/// </summary>
	public static readonly DiagnosticDescription disallowedNonConstants = new() {
		ErrorCode = "RL0007",
		Description = "Labels, variables, parameters and functions may not be used in this context",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0008</c>: Label, variable, parameter or function '{0}' is not declared
	/// </summary>
	public static readonly DiagnosticDescription undeclaredIdentifier = new() {
		ErrorCode = "RL0008",
		Description = "Label, variable, parameter or function '{0}' is not declared",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0009</c>: Functions may only be used in function pointers or function invocations. Did you intend to invoke or point to it?
	/// </summary>
	public static readonly DiagnosticDescription uninvokedFunction = new() {
		ErrorCode = "RL0009",
		Description = "Functions may only be used in function pointers or function invocations. Did you intend to invoke or point to it?",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0010</c>: Functions may not be invoked in this context
	/// </summary>
	public static readonly DiagnosticDescription disallowedFunctionInvocations = new() {
		ErrorCode = "RL0010",
		Description = "Functions may not be invoked in this context",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0011</c>: Function '{0}' is not declared
	/// </summary>
	public static readonly DiagnosticDescription undeclaredFunction = new() {
		ErrorCode = "RL0011",
		Description = "Function '{0}' is not declared",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0012</c>: Cannot invoke label, variable or parameter '{0}'
	/// </summary>
	public static readonly DiagnosticDescription invokeNonFunction = new() {
		ErrorCode = "RL0012",
		Description = "Cannot invoke label, variable or parameter '{0}'",
		Level = DiagnosticLevel.Error
	};
	/// <summary>
	/// <c>RL0013</c>: Cannot point to label, variable or parameter '{0}'
	/// </summary>
	public static readonly DiagnosticDescription nonFunctionPointer = new() {
		ErrorCode = "RL0013",
		Description = "Cannot point to label, variable or parameter '{0}'",
		Level = DiagnosticLevel.Error
	};
	public static readonly DiagnosticDescription unaryOperatorTypeError = new() {
		ErrorCode = "RL0014",
		Description = "Cannot apply operator unary {0} to operand of type {1}",
		Level = DiagnosticLevel.Error
	};
	public static readonly DiagnosticDescription binaryOperatorTypeError = new() {
		ErrorCode = "RL0015",
		Description = "Cannot apply operator unary {0} to operands of type {1} and {2}",
		Level = DiagnosticLevel.Error
	};
	public static readonly DiagnosticDescription divisionByZero = new() {
		ErrorCode = "RL0016",
		Description = "Division by 0",
		Level = DiagnosticLevel.Error
	};
	public static readonly DiagnosticDescription disallowedNonConstantsOnlyLabels = new() {
		ErrorCode = "RL0017",
		Description = "Variables, parameters and functions may not be used in this context",
		Level = DiagnosticLevel.Error
	};

	/*
	public static readonly DiagnosticDescription diagnosticName = new() {
		ErrorCode = "RL0000",
		Description = "",
		Level = DiagnosticLevel.
	};
	*/

}
