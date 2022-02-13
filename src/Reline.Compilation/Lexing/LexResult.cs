﻿using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;

namespace Reline.Compilation.Lexing;

/// <summary>
/// The result of a <see cref="Lexer"/>.
/// </summary>
/// <param name="Tokens">The lexed syntax tokens.</param>
/// <param name="Diagnostics">The diagnostics generated by the lexer.</param>
internal readonly record struct LexResult(
	ImmutableArray<SyntaxToken> Tokens,
	ImmutableArray<Diagnostic> Diagnostics
);
