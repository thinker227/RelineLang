using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Syntax.Nodes;

public static class SyntaxNodeExtensions {

	/// <summary>
	/// Adds a diagnostic to a <see cref="SyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TNode">The type of the syntax node.</typeparam>
	/// <param name="node">The source node.</param>
	/// <param name="diagnostic">The diagnostic to add.</param>
	/// <returns>A new syntax node of type <typeparamref name="TNode"/>
	/// with <paramref name="diagnostic"/> added to its diagnostics.</returns>
	public static TNode AddDiagnostic<TNode>(this TNode node, Diagnostic diagnostic) where TNode : SyntaxNode {
		var newDiagnostics = node.Diagnostics.IsDefault ?
			ImmutableArray.Create(diagnostic) :
			node.Diagnostics.Add(diagnostic);
		return node with { Diagnostics = newDiagnostics };
	}
	/// <summary>
	/// Adds a collection of diagnostics to a <see cref="SyntaxNode"/>.
	/// </summary>
	/// <typeparam name="TNode">The type of the syntax node.</typeparam>
	/// <param name="node">The source node.</param>
	/// <param name="diagnostics">The collection of diagnostics to add.</param>
	/// <returns>A new syntax node of type <typeparamref name="TNode"/>
	/// with <paramref name="diagnostics"/> added to its diagnostics.</returns>
	public static TNode AddDiagnostics<TNode>(this TNode node, IEnumerable<Diagnostic> diagnostics) where TNode : SyntaxNode {
		var newDiagnostics = node.Diagnostics.IsDefault ?
			ImmutableArray.CreateRange(diagnostics) :
			node.Diagnostics.AddRange(diagnostics);
		return node with { Diagnostics = newDiagnostics };
	}

	/// <summary>
	/// Gets the <see cref="TextSpan"/> of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the text span of.</param>
	/// <returns>The <see cref="TextSpan"/> of <paramref name="node"/>.</returns>
	/// <exception cref="NotSupportedException">The syntax node type is not supported.</exception>
	public static TextSpan GetTextSpan(this ISyntaxNode node) =>
		node switch {
			ProgramSyntax program => new(
				GetTextSpan(program.Lines[0]).Start,
				GetTextSpan(program.Lines[^1]).End),
			LineSyntax line => new(
				line.Label?.GetTextSpan().Start ??
				line.Statement?.GetTextSpan().Start ??
				line.NewlineToken.Span.Start,
				line.NewlineToken.Span.End),
			LabelSyntax label => new(
				label.Identifier.Span.Start,
				label.ColonToken.Span.End),

			AssignmentStatementSyntax assignment => new(
				assignment.Identifier.Span.Start,
				assignment.Initializer.GetTextSpan().End),
			IManipulationStatementSyntax manipulation => new(
				manipulation.SourceKeyword.Span.Start,
				manipulation.Target.GetTextSpan().End),
			ExpressionStatementSyntax expression => expression.GetTextSpan(),

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

			AssignmentStatementSyntax assignment =>
				ImmutableArray.Create<ISyntaxNode>(assignment.Initializer),
			IManipulationStatementSyntax manipulation =>
				ImmutableArray.Create<ISyntaxNode>(manipulation.Source, manipulation.Target),
			ExpressionStatementSyntax expression =>
				ImmutableArray.Create<ISyntaxNode>(expression.Expression),

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
			ITokenExpressionSyntax =>
				ImmutableArray<ISyntaxNode>.Empty,

			_ => throw new NotSupportedException($"Cannot retrieve children of node type {node.GetType().Name}")
		};
	/// <summary>
	/// Recursively gets all the child nodes of a syntax node.
	/// </summary>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The recursive child nodes of <paramref name="node"/>.</returns>
	public static ImmutableArray<ISyntaxNode> GetAllChildren(this ISyntaxNode node) {
		var children = node.GetChildren();
		var subchildren = children.SelectMany(n => n.GetAllChildren());
		return children.AddRange(subchildren);
	}
	/// <summary>
	/// Recursively gets all the child nodes of a specified type of a syntax node.
	/// </summary>
	/// <typeparam name="TNode">The type to get the nodes of.</typeparam>
	/// <param name="node">The node to get the children of.</param>
	/// <returns>The recursive child nodes of <paramref name="node"/>
	/// of type <typeparamref name="TNode"/>.</returns>
	public static ImmutableArray<TNode> GetAllChildren<TNode>(this ISyntaxNode node) where TNode : ISyntaxNode =>
		GetAllChildren(node).OfType<TNode>().ToImmutableArray();

}
