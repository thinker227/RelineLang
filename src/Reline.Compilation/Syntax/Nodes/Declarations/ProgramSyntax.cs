namespace Reline.Compilation.Syntax.Nodes;

public sealed record class ProgramSyntax(
	ImmutableArray<LineSyntax> Lines
) : SyntaxNode;
