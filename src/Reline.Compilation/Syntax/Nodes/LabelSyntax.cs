﻿namespace Reline.Compilation.Syntax.Nodes;

public sealed record class LabelSyntax(
	IdentifierSyntax Identifier,
	SyntaxToken ColonToken
) : SyntaxNode;