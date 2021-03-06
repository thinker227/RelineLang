using System.Runtime.CompilerServices;
using Reline.Compilation;
using Reline.Compilation.Binding;
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
	public static FunctionDeclarationStatementSymbol FunctionIs(this FunctionDeclarationStatementSymbol declaration, Action<FunctionSymbol> action) {
		Assert.NotNull(declaration.Function);
		action(declaration.Function!);
		return declaration;
	}
	public static FunctionPointerExpressionSymbol FunctionIs(this FunctionPointerExpressionSymbol pointer, Action<FunctionSymbol> action) {
		Assert.NotNull(pointer.Function);
		action(pointer.Function!);
		return pointer;
	}
	public static ParameterSymbol FunctionIs(this ParameterSymbol parameter, Action<FunctionSymbol> action) {
		Assert.NotNull(parameter.Function);
		action(parameter.Function!);
		return parameter;
	}
	public static ReturnStatementSymbol FunctionIs(this ReturnStatementSymbol @return, Action<FunctionSymbol> action) {
		Assert.NotNull(@return.Function);
		action(@return.Function!);
		return @return;
	}
	public static ReturnStatementSymbol Functionis(this ReturnStatementSymbol @return, FunctionSymbol? function) {
		Assert.Equal(function, @return.Function);
		return @return;
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
	
	public static FunctionSymbol RangeIs(this FunctionSymbol function, Action<RangeValue> action) {
		action(function.Range);
		return function;
	}
	public static FunctionSymbol ArityIs(this FunctionSymbol function, int arity) {
		Assert.Equal(arity, function.Arity);
		return function;
	}
	public static FunctionSymbol ParametersAre(this FunctionSymbol function, Action<EnumerableAssert<ParameterSymbol>> action) {
		var assert = function.Parameters.AssertEnumerable();
		action(assert);
		return function;
	}
	public static FunctionSymbol ParametersAre(this FunctionSymbol function, int count) {
		Assert.Equal(count, function.Parameters.Count);
		return function;
	}
	public static NativeFunctionSymbol FunctionTypeIs(this NativeFunctionSymbol function, NativeFunction functionType) {
		Assert.Equal(function.FunctionType, functionType);
		return function;
	}
	
	public static LiteralExpressionSymbol HasValue(this LiteralExpressionSymbol literal, BoundValue value) {
		Assert.Equal(value, literal.Literal);
		return literal;
	}
	public static UnaryExpressionSymbol OperatorTypeIs(this UnaryExpressionSymbol expression, UnaryOperatorType operatorType) {
		Assert.Equal(operatorType, expression.OperatorType);
		return expression;
	}
	public static BinaryExpressionSymbol OperatorTypeIs(this BinaryExpressionSymbol expression, BinaryOperatorType operatorType) {
		Assert.Equal(operatorType, expression.OperatorType);
		return expression;
	}
	public static KeywordExpressionSymbol KeywordIs(this KeywordExpressionSymbol expression, KeywordExpressionType keywordType) {
		Assert.Equal(keywordType, expression.KeywordType);
		return expression;
	}
	public static FunctionInvocationExpressionSymbol ArgumentsAre(this FunctionInvocationExpressionSymbol invocation, Action<EnumerableAssert<IExpressionSymbol>> action) {
		var assert = invocation.Arguments.AssertEnumerable();
		action(assert);
		return invocation;
	}

	public static LineSymbol LineNumberIs(this LineSymbol symbol, int lineNumber, [CallerArgumentExpression("symbol")] string expression = "") {
		Assert.Equal(lineNumber, symbol.LineNumber);
		return symbol;
	}

	public static RangeValue StartIs(this RangeValue range, int start) {
		Assert.Equal(start, range.Start);
		return range;
	}
	public static RangeValue EndIs(this RangeValue range, int end) {
		Assert.Equal(end, range.End);
		return range;
	}
	public static RangeValue LengthIs(this RangeValue range, int length) {
		Assert.Equal(length, range.Length);
		return range;
	}

}
