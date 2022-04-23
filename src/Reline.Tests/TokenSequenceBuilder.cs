using System.Collections;
using Reline.Compilation.Syntax;

namespace Reline.Tests;

public sealed class TokenSequenceBuilder {

	private readonly ImmutableArray<SyntaxToken> tokens;
	private readonly ImmutableArray<SyntaxTrivia> trivia;
	private readonly string? constantTrivia;

	private int End => Math.Max(
		tokens.Length == 0 ? 0 : tokens[^1].Span.End,
		trivia.Length == 0 ? 0 : trivia[^1].Span.End);



	private TokenSequenceBuilder(ImmutableArray<SyntaxToken> tokens, ImmutableArray<SyntaxTrivia> trivia, string? constantTrivia) {
		this.tokens = tokens;
		this.trivia = trivia;
		this.constantTrivia = constantTrivia;
	}



	public static TokenSequenceBuilder Create() =>
		Create(null);
	public static TokenSequenceBuilder Create(string? constantTrivia) =>
		new(ImmutableArray<SyntaxToken>.Empty, ImmutableArray<SyntaxTrivia>.Empty, constantTrivia);

	private SyntaxTrivia CreateTrivia(string text) =>
		new(TextSpan.FromLength(End, text.Length), text);
	private TokenSequenceBuilder Add(SyntaxToken token) {
		var trivia = this.trivia;
		if (constantTrivia is not null) trivia = trivia.Add(CreateTrivia(constantTrivia));
		if (trivia.Length > 0) token = token.WithLeadingTrivia(trivia);
		return new(tokens.Add(token), ImmutableArray<SyntaxTrivia>.Empty, constantTrivia);
	}
	private TokenSequenceBuilder Add(SyntaxTrivia trivia) =>
		new(tokens, this.trivia.Add(trivia), constantTrivia);
	public TokenSequenceBuilder Add(SyntaxType type, string text) =>
		Add(type, text, null);
	public TokenSequenceBuilder Add(SyntaxType type, string text, object? literal) {
		var span = TextSpan.FromLength(End, text.Length);
		SyntaxToken token = new(type, span, text, literal);
		return Add(token);
	}
	public TokenSequenceBuilder Num(int num) =>
		Num(num.ToString(), num);
	public TokenSequenceBuilder Num(string text, int num) =>
		Add(SyntaxType.NumberLiteral, text, num);
	public TokenSequenceBuilder Str(string str) =>
		Add(SyntaxType.StringLiteral, $"\"{str}\"", str);
	public TokenSequenceBuilder Trivia(string text) =>
		Add(CreateTrivia(text));
	public IEnumerable<SyntaxToken> EOF() {
		var span = TextSpan.FromEmpty(End);
		SyntaxToken token = new(SyntaxType.EndOfFile, span, "", null);
		return Add(token).tokens;
	}

}
