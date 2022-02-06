namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a generic expression visitor.
/// </summary>
/// <typeparam name="T">The type the visitor returns.</typeparam>
public interface IExpressionVisitor<out T> {

	T VisitUnary(UnaryExpressionSymbol symbol);
	T VisitBinary(BinaryExpressionSymbol symbol);
	T VisitKeyword(KeywordExpressionSymbol symbol);
	T VisitLiteral(LiteralExpressionSymbol symbol);
	T VisitFunctionInvocation(FunctionInvocationExpressionSymbol symbol);
	T VisitFunctionPointer(FunctionPointerExpressionSymbol symbol);
	T VisitVariable(IdentifierExpressionSymbol symbol);

}
