namespace Reline.Compilation.Syntax.Nodes;

public static class SyntaxNodeExtensions {

	/// <summary>
	/// Gets the <see cref="TextSpan"/> of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the text span of.</param>
	/// <returns>The <see cref="TextSpan"/> of <paramref name="node"/>.</returns>
	/// <exception cref="NotSupportedException">The syntax node type is not supported.</exception>
	public static TextSpan GetTextSpan(this ISyntaxNode node) =>
		node.Accept(TextSpanEvaluator.Instance);

	/// <summary>
	/// Recursively gets all the child nodes of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The recursive child nodes of <paramref name="node"/>.</returns>
	public static IEnumerable<ISyntaxNode> GetAllDescendants(this ISyntaxNode node) =>
		node.GetChildren()
			.SelectMany(n => n.GetAllDescendants().Prepend(n))
			.ToImmutableArray();
	/// <summary>
	/// Recursively gets all the child nodes of a specified type of a syntax node.
	/// </summary>
	/// <typeparam name="TNode">The type to get the nodes of.</typeparam>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The recursive child nodes of <paramref name="node"/>
	/// of type <typeparamref name="TNode"/>.</returns>
	public static IEnumerable<TNode> GetAllDescendants<TNode>(this ISyntaxNode node) where TNode : ISyntaxNode =>
		GetAllDescendants(node).OfType<TNode>();

	private sealed class TextSpanEvaluator : ISyntaxVisitor<TextSpan> {

		public static TextSpanEvaluator Instance { get; } = new();
		private TextSpanEvaluator() { }

		public TextSpan VisitProgram(ProgramSyntax syntax) => new(
			syntax.Lines[0].GetTextSpan().Start,
			syntax.Lines[^1].GetTextSpan().End);
		public TextSpan VisitLine(LineSyntax syntax) => new(
			syntax.Label?.GetTextSpan().Start ??
			syntax.Statement?.GetTextSpan().Start ??
			syntax.NewlineToken.Span.Start,
			syntax.NewlineToken.Span.End);
		public TextSpan VisitLabel(LabelSyntax syntax) => new(
			syntax.Identifier.Span.Start,
			syntax.ColonToken.Span.End);
		public TextSpan VisitParameterList(ParameterListSyntax syntax) => new(
			syntax.OpenBracketToken.Span.Start,
			syntax.CloseBracketToken.Span.End);

		public TextSpan VisitAssignmentStatement(AssignmentStatementSyntax syntax) => new(
			syntax.Identifier.Span.Start,
			syntax.Initializer.GetTextSpan().End);
		public TextSpan VisitExpressionStatement(ExpressionStatementSyntax syntax) =>
			syntax.GetTextSpan();
		public TextSpan VisitMoveStatement(MoveStatementSyntax syntax) => new(
			syntax.MoveKeyword.Span.Start,
			syntax.Target.GetTextSpan().End);
		public TextSpan VisitSwapStatement(SwapStatementSyntax syntax) => new(
			syntax.SwapKeyword.Span.Start,
			syntax.Target.GetTextSpan().End);
		public TextSpan VisitCopyStatement(CopyStatementSyntax syntax) => new(
			syntax.CopyKeyword.Span.Start,
			syntax.Target.GetTextSpan().End);
		public TextSpan VisitFunctionDeclarationStatement(FunctionDeclarationStatementSyntax syntax) => new(
			syntax.FunctionKeyword.Span.Start,
			syntax.ParameterList?.GetTextSpan().End ??
			syntax.Body.GetTextSpan().End);
		public TextSpan VisitReturnStatement(ReturnStatementSyntax syntax) => new(
			syntax.ReturnKeyword.Span.Start,
			syntax.Expression.GetTextSpan().End);

		public TextSpan VisitUnaryExpression(UnaryExpressionSyntax syntax) => new(
			syntax.OperatorToken.Span.Start,
			syntax.Expression.GetTextSpan().End);
		public TextSpan VisitBinaryExpression(BinaryExpressionSyntax syntax) => new(
			syntax.Left.GetTextSpan().Start,
			syntax.Right.GetTextSpan().End);
		public TextSpan VisitKeywordExpression(KeywordExpressionSyntax syntax) =>
			syntax.Keyword.Span;
		public TextSpan VisitLiteralExpression(LiteralExpressionSyntax syntax) =>
			syntax.Literal.Span;
		public TextSpan VisitGroupingExpression(GroupingExpressionSyntax syntax) => new(
			syntax.OpenBracketToken.Span.Start,
			syntax.CloseBracketToken.Span.End);
		public TextSpan VisitIdentifierExpression(IdentifierExpressionSyntax syntax) =>
			syntax.Identifier.Span;
		public TextSpan VisitFunctionInvocationExpression(FunctionInvocationExpressionSyntax syntax) => new(
			syntax.Identifier.Span.Start,
			syntax.CloseBracketToken.Span.End);
		public TextSpan VisitFunctionPointerExpression(FunctionPointerExpressionSyntax syntax) => new(
			syntax.StarToken.Span.Start,
			syntax.Identifier.Span.End);

	}

}
