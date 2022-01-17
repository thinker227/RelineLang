using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax.Nodes;

public static class SyntaxNodeExtensions {

	/// <summary>
	/// Gets the <see cref="TextSpan"/> of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the text span of.</param>
	/// <returns>The <see cref="TextSpan"/> of <paramref name="node"/>.</returns>
	/// <exception cref="NotSupportedException">The syntax node type is not supported.</exception>
	public static TextSpan GetTextSpan(this ISyntaxNode node) =>
		node switch {
			ProgramSyntax program => new(
				program.Lines[0].GetTextSpan().Start,
				program.Lines[^1].GetTextSpan().End),
			LineSyntax line => new(
				line.Label?.GetTextSpan().Start ??
				line.Statement?.GetTextSpan().Start ??
				line.NewlineToken.Span.Start,
				line.NewlineToken.Span.End),
			LabelSyntax label => new(
				label.Identifier.Span.Start,
				label.ColonToken.Span.End),
			ParameterListSyntax pList => new(
				pList.OpenBracketToken.Span.Start,
				pList.CloseBracketToken.Span.End),
			TypedIdentifierSyntax typedI => new(
				typedI.Type.GetTextSpan().Start,
				typedI.Identifier.Span.End),

			ExpressionStatementSyntax expression => expression.GetTextSpan(),
			AssignmentStatementSyntax assignment => new(
				assignment.Identifier.Span.Start,
				assignment.Initializer.GetTextSpan().End),
			IManipulationStatementSyntax manipulation => new(
				manipulation.SourceKeyword.Span.Start,
				manipulation.Target.GetTextSpan().End),
			ReturnStatementSyntax @return => new(
				@return.ReturnKeyword.Span.Start,
				@return.Expression.GetTextSpan().End),
			FunctionDeclarationStatementSyntax function => new(
				function.FunctionKeyword.Span.Start,
				function.ParameterList?.GetTextSpan().End ??
				function.ReturnType?.GetTextSpan().End ??
				function.Body.GetTextSpan().End),

			IUnaryExpressionSyntax unary => new(
				unary.UnaryToken.Span.Start,
				unary.Expression.GetTextSpan().End),
			IBinaryExpressionSyntax binary => new(
				binary.Left.GetTextSpan().Start,
				binary.Right.GetTextSpan().End),
			ITokenExpressionSyntax token => token.Token.Span,
			GroupingExpressionSyntax grouping => new(
				grouping.OpenBracketToken.Span.Start,
				grouping.CloseBracketToken.Span.End),
			FunctionInvocationExpressionSyntax invocation => new(
				invocation.Identifier.Span.Start,
				invocation.CloseBracketToken.Span.End),
			IdentifierExpressionSyntax identifier => identifier.Identifier.Span,
			UnaryLinePointerExpressionSyntax pointer => new(
				pointer.StarToken.Span.Start,
				pointer.CloseSquareToken.Span.End),

			ITokenTypeSyntax token => token.Token.Span,

			_ => throw new NotSupportedException($"Cannot retrieve text span of node type {node.GetType().Name}")
		};

	/// <summary>
	/// Gets the child nodes of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The child nodes of <paramref name="node"/>.</returns>
	/// <exception cref="NotSupportedException">The syntax node type is not supported.</exception>
	public static ImmutableArray<ISyntaxNode> GetChildren(this ISyntaxNode node) =>
		node switch {
			ProgramSyntax program =>
				program.Lines.As<ISyntaxNode>(),
			LineSyntax line =>
				ImmutableArray<ISyntaxNode>.Empty
				.AddNotNull(line.Label)
				.AddNotNull(line.Statement),
			ParameterListSyntax pList =>
				pList.Parameters.As<ISyntaxNode>(),

			ExpressionStatementSyntax expression =>
				ImmutableArray.Create<ISyntaxNode>(expression.Expression),
			AssignmentStatementSyntax assignment =>
				ImmutableArray.Create<ISyntaxNode>(assignment.Initializer),
			IManipulationStatementSyntax manipulation =>
				ImmutableArray.Create<ISyntaxNode>(manipulation.Source, manipulation.Target),
			ReturnStatementSyntax @return =>
				ImmutableArray.Create<ISyntaxNode>(@return.Expression),
			FunctionDeclarationStatementSyntax function =>
				ImmutableArray.Create<ISyntaxNode>(function.Body)
				.AddNotNull(function.ParameterList),

			IUnaryExpressionSyntax unary =>
				ImmutableArray.Create<ISyntaxNode>(unary.Expression),
			IBinaryExpressionSyntax binary =>
				ImmutableArray.Create<ISyntaxNode>(binary.Left, binary.Right),
			GroupingExpressionSyntax grouping =>
				ImmutableArray.Create<ISyntaxNode>(grouping.Expression),
			FunctionInvocationExpressionSyntax invocation =>
				invocation.Arguments.As<ISyntaxNode>(),
			IdentifierExpressionSyntax identifier =>
				ImmutableArray.Create<ISyntaxNode>(identifier),
			UnaryLinePointerExpressionSyntax pointer =>
				ImmutableArray.Create<ISyntaxNode>(pointer.Expression),

			LabelSyntax or
			TypedIdentifierSyntax or
			ITokenExpressionSyntax or
			ITokenTypeSyntax =>
				ImmutableArray<ISyntaxNode>.Empty,

			_ => throw new NotSupportedException($"Cannot retrieve children of node type {node.GetType().Name}")
		};
	/// <summary>
	/// Recursively gets all the child nodes of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The recursive child nodes of <paramref name="node"/>.</returns>
	public static ImmutableArray<ISyntaxNode> GetAllDescendants(this ISyntaxNode node) {
		var children = node.GetChildren();
		var subchildren = children.SelectMany(n => n.GetAllDescendants());
		return children.AddRange(subchildren);
	}
	/// <summary>
	/// Recursively gets all the child nodes of a specified type of a syntax node.
	/// </summary>
	/// <typeparam name="TNode">The type to get the nodes of.</typeparam>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The recursive child nodes of <paramref name="node"/>
	/// of type <typeparamref name="TNode"/>.</returns>
	public static ImmutableArray<TNode> GetAllDescendants<TNode>(this ISyntaxNode node) where TNode : ISyntaxNode =>
		GetAllDescendants(node).OfType<TNode>().ToImmutableArray();

}
