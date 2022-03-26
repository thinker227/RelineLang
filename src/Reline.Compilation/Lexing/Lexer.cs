using Reline.Compilation.Syntax;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Lexing;

/// <summary>
/// Lexes a string of character into a series of syntax tokens.
/// </summary>
internal sealed class Lexer {

	private readonly string source;
	private readonly SourceViewer viewer;
	private int lexemeStartPosition;
	private readonly List<Diagnostic> diagnostics;
	private TextSpan CurrentSpan =>
		new(lexemeStartPosition, viewer.Position);



	private Lexer(string source) {
		this.source = source;
		viewer = new(source);
		diagnostics = new();
	}



	/// <summary>
	/// Lexes a source string and returns a collection of syntax tokens.
	/// </summary>
	/// <param name="source">The source string.</param>
	/// <returns>A <see cref="LexResult"/> containing
	/// the lexed tokens and resulting diagnostics.</returns>
	public static LexResult LexSource(string source) {
		Lexer lexer = new(source);
		var tokens = lexer.LexAll();
		var diagnostics = lexer.diagnostics.ToImmutableArray();
		return new LexResult(tokens, diagnostics);
	}
	private ImmutableArray<SyntaxToken> LexAll() {
		List<SyntaxToken> tokens = new();
		List<SyntaxTrivia> trivia = new();
		
		void addToken(SyntaxToken token) {
			if (trivia.Count > 0) {
				token = token.WithLeadingTrivia(trivia);
				trivia.Clear();
			}
			tokens.Add(token);
		}

		while (!viewer.IsAtEnd) {
			var token = LexNext();

			if (IsTriviaType(token.Type))
				trivia.Add(token.ToSyntaxTrivia());
			else addToken(token);
		}

		addToken(new(SyntaxType.EndOfFile, TextSpan.FromEmpty(source.Length), "", null));

		return tokens.ToImmutableArray();
	}
	private static bool IsTriviaType(SyntaxType type) => type is
		SyntaxType.Whitespace or
		SyntaxType.Comment or
		SyntaxType.Unknown;
	private SyntaxToken LexNext() {
		lexemeStartPosition = viewer.Position;
		char current = viewer.Current;

		var characterToken = GetCharacterToken(current);
		if (characterToken is not null) return characterToken.Value;

		if (SyntaxRules.IsQuote(current)) return GetStringLiteral();
		if (SyntaxRules.IsNumeric(current)) return GetNumericLiteral();
		if (SyntaxRules.CanBeginIdentifier(current)) return GetIdentifierOrKeywordToken();
		if (SyntaxRules.IsComment(viewer.GetString(2))) return GetComment();
		if (SyntaxRules.IsWhitespace(current)) {
			viewer.Advance();
			return CreateToken(SyntaxType.Whitespace);
		}

		viewer.Advance();
		var diagnostic = CompilerDiagnostics.unexpectedCharacter
			.ToDiagnostic(CurrentSpan, current);
		diagnostics.Add(diagnostic);
		return CreateToken(SyntaxType.Unknown);
	}

	private SyntaxToken? GetCharacterToken(char c) {
		SyntaxType? single = c switch {
			':' => SyntaxType.ColonToken,
			'=' => SyntaxType.EqualsToken,
			'<' => SyntaxType.LesserThanToken,
			'+' => SyntaxType.PlusToken,
			'-' => SyntaxType.MinusToken,
			'*' => SyntaxType.StarToken,
			'\\' => SyntaxType.BackslashToken,
			'%' => SyntaxType.PercentToken,
			'(' => SyntaxType.OpenBracketToken,
			')' => SyntaxType.CloseBracketToken,
			'\n' => SyntaxType.NewlineToken,

			_ => null
		};
		if (single is not null) {
			viewer.Advance();
			return CreateToken(single.Value);
		}

		switch (c) {
			case '.':
				if (viewer.Next != '.') break;
				viewer.AdvanceDistance(2);
				return CreateToken(SyntaxType.DotDotToken);
			case '/':
				if (viewer.Next == '/') return GetComment();
				viewer.Advance();
				return CreateToken(SyntaxType.SlashToken);
		};

		return null;
	}
	private SyntaxToken GetNumericLiteral() {
		int literal = 0;
		while (!viewer.IsAtEnd && SyntaxRules.IsNumeric(viewer.Current))
			literal = literal * 10 + viewer.AdvanceCurrent() - '0';

		return CreateToken(SyntaxType.NumberLiteral, literal);
	}
	private SyntaxToken GetStringLiteral() {
		viewer.Advance();

		int startPosition = viewer.Position;
		viewer.AdvanceWhile(c => !SyntaxRules.IsQuote(c));
		int endPosition = viewer.Position;
		string literal = source[startPosition..endPosition];

		viewer.Advance();

		return CreateToken(SyntaxType.StringLiteral, literal);
	}

	private SyntaxToken GetIdentifierOrKeywordToken() {
		string str = GetIdentifierOrKeywordString();
		var keyword = GetKeywordType(str);
		if (keyword is not null) return CreateToken(keyword.Value);
		return CreateToken(SyntaxType.Identifier);
	}
	private string GetIdentifierOrKeywordString() {
		int startPosition = viewer.Position;
		viewer.Advance();
		viewer.AdvanceWhile(SyntaxRules.IsIdentifierValid);
		int endPosition = viewer.Position;
		return source[startPosition..endPosition];
	}
	private static SyntaxType? GetKeywordType(string s) =>
		s switch {
			"here" => SyntaxType.HereKeyword,
			"start" => SyntaxType.StartKeyword,
			"end" => SyntaxType.EndKeyword,
			"move" => SyntaxType.MoveKeyword,
			"swap" => SyntaxType.SwapKeyword,
			"copy" => SyntaxType.CopyKeyword,
			"to" => SyntaxType.ToKeyword,
			"with" => SyntaxType.WithKeyword,
			"return" => SyntaxType.ReturnKeyword,
			"function" => SyntaxType.FunctionKeyword,

			_ => null
		};

	private SyntaxToken GetComment() {
		viewer.AdvanceWhile(c => c != '\n');
		return CreateToken(SyntaxType.Comment);
	}

	private SyntaxToken CreateToken(SyntaxType type) =>
		CreateToken(type, null);
	private SyntaxToken CreateToken(SyntaxType type, object? literal) =>
		new(type, CurrentSpan, source.Substring(CurrentSpan), literal);

}
