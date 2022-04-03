using Reline.Compilation.Symbols;

namespace Reline.Tests;

public static class SymbolExtensions {

	public static LineSymbol LabelIs(this LineSymbol line, Action<LabelSymbol> action) {
		Assert.NotNull(line.Label);
		action(line.Label!);
		return line;
	}
	public static AssignmentStatementSymbol VariableIs(this AssignmentStatementSymbol assignment, Action<IVariableSymbol> action) {
		Assert.NotNull(assignment.Variable);
		action(assignment.Variable!);
		return assignment;
	}
	public static FunctionInvocationExpressionSymbol FunctionIs(this FunctionInvocationExpressionSymbol invocation, Action<IFunctionSymbol> action) {
		Assert.NotNull(invocation.Function);
		action(invocation.Function!);
		return invocation;
	}
	public static IdentifierExpressionSymbol IdentifierIs(this IdentifierExpressionSymbol identifier, Action<IIdentifiableSymbol> action) {
		Assert.NotNull(identifier.Identifier);
		action(identifier.Identifier!);
		return identifier;
	}

	public static TSymbol IdentifierIs<TSymbol>(this TSymbol identifiable, string identifier) where TSymbol : IIdentifiableSymbol {
		Assert.Equal(identifier, identifiable.Identifier);
		return identifiable;
	}
	public static TSymbol HasReferences<TSymbol>(this TSymbol identifiable, int count) where TSymbol : IDefinedIdentifiableSymbol {
		Assert.Equal(count, identifiable.References.Count);
		return identifiable;
	}

	public static LiteralExpressionSymbol HasValue(this LiteralExpressionSymbol literal, BoundValue value) {
		Assert.Equal(value, literal.Literal);
		return literal;
	}

	public static LineSymbol HasLineNumber(this LineSymbol symbol, int lineNumber) {
		Assert.Equal(lineNumber, symbol.LineNumber);
		return symbol;
	}

}
