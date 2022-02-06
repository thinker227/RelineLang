namespace Reline.Compilation.Syntax.Nodes;

public interface ISyntaxVisitor<T> {

	T VisitProgram(ProgramSyntax syntax);
	T VisitLine(LineSyntax syntax);
	T VisitLabel(LabelSyntax syntax);
	T VisitParameterList(ParameterListSyntax syntax);

	T VisitAssignmentStatement(AssignmentStatementSyntax syntax);
	T VisitExpressionStatement(ExpressionStatementSyntax syntax);
	T VisitMoveStatement(MoveStatementSyntax syntax);
	T VisitSwapStatement(SwapStatementSyntax syntax);
	T VisitCopyStatement(CopyStatementSyntax syntax);
	T VisitFunctionDeclarationStatement(FunctionDeclarationStatementSyntax syntax);
	T VisitReturnStatement(ReturnStatementSyntax syntax);

	T VisitUnaryExpression(UnaryExpressionSyntax syntax);
	T VisitBinaryExpression(BinaryExpressionSyntax syntax);
	T VisitKeywordExpression(KeywordExpressionSyntax syntax);
	T VisitLiteralExpression(LiteralExpressionSyntax syntax);
	T VisitGroupingExpression(GroupingExpressionSyntax syntax);
	T VisitIdentifierExpression(IdentifierExpressionSyntax syntax);
	T VisitFunctionInvocationExpression(FunctionInvocationExpressionSyntax syntax);
	T VisitFunctionPointerExpression(FunctionPointerExpressionSyntax syntax);

}
