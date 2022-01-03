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
	public static TNode AddDiagnostic<TNode>(this TNode node, IEnumerable<Diagnostic> diagnostics) where TNode : SyntaxNode {
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
			ProgramSyntax program =>
				new(GetTextSpan(program.Lines[0]).Start, GetTextSpan(program.Lines[^1]).End),
			LineSyntax line => new(
				line.Label?.GetTextSpan().Start ??
				line.Statement?.GetTextSpan().Start ??
				line.NewlineToken.Span.Start,
				line.NewlineToken.Span.End),
			LabelSyntax label => new(
				label.Identifier.Name.Span.Start,
				label.ColonToken.Span.End),
			IdentifierSyntax identifier => identifier.Name.Span,

			AssignmentStatementSyntax assignment => new(
				assignment.Identifier.Name.Span.Start,
				assignment.Initializer.GetTextSpan().End),
			MoveStatementSyntax move => new(
				move.MoveKeyword.Span.Start,
				move.Target.GetTextSpan().End),
			SwapStatementSyntax swap => new(
				swap.SwapKeyword.Span.Start,
				swap.Target.GetTextSpan().End),
			CopyStatementSyntax copy => new(
				copy.CopyKeyword.Span.Start,
				copy.Target.GetTextSpan().End),
			ExpressionStatementSyntax expression => expression.GetTextSpan(),

			IBinaryExpressionSyntax binary => new(
				binary.Left.GetTextSpan().Start,
				binary.Right.GetTextSpan().End),
			IUnaryExpressionSyntax unary => new(
				unary.UnaryToken.Span.Start,
				unary.Expression.GetTextSpan().End),
			ITokenExpressionSyntax token => token.Token.Span,
			GroupingExpressionSyntax grouping => new(
				grouping.OpenBracketToken.Span.Start,
				grouping.CloseBracketToken.Span.End),
			FunctionInvocationExpressionSyntax invocation => new(
				invocation.Identifier.Name.Span.Start,
				invocation.CloseBracketToken.Span.End),
			IdentifierExpressionSyntax identifier => identifier.Identifier.Name.Span,
			UnaryLinePointerExpressionSyntax pointer => new(
				pointer.StarToken.Span.Start,
				pointer.CloseSquareToken.Span.End),

			_ => throw new NotSupportedException($"Cannot retrieve text span of node type {node.GetType().Name}")
		};

}
