namespace Reline.Compilation.Syntax.Nodes;

public sealed record class FunctionDeclarationStatementSyntax(
	SyntaxToken FunctionKeyword,
	SyntaxToken Identifier,
	IExpressionSyntax Body,
	ITypeSyntax? ReturnType,
	ParameterListSyntax? ParameterList
) : SyntaxNode, IStatementSyntax;
