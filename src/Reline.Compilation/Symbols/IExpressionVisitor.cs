namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a generic expression visitor.
/// </summary>
/// <typeparam name="T">The type the visitor returns.</typeparam>
public interface IExpressionVisitor<out T> {

	T VisitUnaryPlus(UnaryPlusExpressionSymbol symbol);
	T VisitUnaryNegation(UnaryNegationExpressionSymbol symbol);
	T VisitUnaryLinePointer(UnaryLinePointerExpressionSymbol symbol);
	T VisitUnaryFunctionPointer(UnaryFunctionPointerExpressionSymbol symbol);

	T VisitBinaryAddition(BinaryAdditionExpressionSymbol symbol);
	T VisitBinarySubtraction(BinarySubtractionExpressionSymbol symbol);
	T VisitBinaryMultiplication(BinaryMultiplicationExpressionSymbol symbol);
	T VisitBinaryDivision(BinaryDivisionExpressionSymbol symbol);
	T VisitBinaryModulo(BinaryModuloExpressionSymbol symbol);
	T VisitBinaryConcatenation(BinaryConcatenationExpressionSymbol symbol);

	T VisitStart(StartExpressionSymbol symbol);
	T VisitEnd(EndExpressionSymbol symbol);
	T VisitHere(HereExpressionSymbol symbol);

	T VisitLiteral(LiteralExpressionSymbol symbol);
	T VisitGrouping(GroupingExpressionSymbol symbol);
	T VisitRange(RangeExpressionSymbol symbol);
	T VisitFunctionInvocation(FunctionInvocationExpressionSymbol symbol);
	T VisitVariable(VariableExpressionSymbol symbol);

}
