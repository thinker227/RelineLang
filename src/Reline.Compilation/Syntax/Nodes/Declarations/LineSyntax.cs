namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LineSyntax(
	LabelSyntax? Label,
	IStatementSyntax? Statement,
	SyntaxToken NewlineToken
) : SyntaxNode, ISyntaxNode;
