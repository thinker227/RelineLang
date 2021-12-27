namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionInvocationExpressionSyntax(
	IdentifierSyntax Identifier,
	ImmutableArray<IExpressionSyntax> Arguments
) : SyntaxNode, IExpressionSyntax;
