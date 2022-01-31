namespace Reline.Compilation.Diagnostics;

internal static class CompilerDiagnostics {

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

}
