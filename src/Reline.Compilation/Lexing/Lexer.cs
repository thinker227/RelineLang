using Reline.Compilation.Syntax;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Lexing;

/// <summary>
/// Lexes a string of character into a series of syntax tokens.
/// </summary>
public sealed class Lexer {

	private readonly SourceViewer viewer;
	private int lexemeStartPosition;
	private readonly List<Diagnostic> diagnostics;



	private TextSpan CurrentSpan =>
		new(lexemeStartPosition, viewer.Position);
	/// <summary>
	/// The source text being lexed.
	/// </summary>
	public string Source { get; }



	/// <summary>
	/// Initializes a new <see cref="Lexer"/> instance.
	/// </summary>
	/// <param name="source">The source text being lexed.</param>
	public Lexer(string source) {
		viewer = new(source);
		diagnostics = new();
		Source = source;
	}



	/// <summary>
	/// Lexes all characters in the source text.
	/// </summary>
	/// <returns>A <see cref="LexResult"/> containing the result of the operation.</returns>
	public IOperationResult<ImmutableArray<SyntaxToken>> LexAll() {
		List<SyntaxToken> tokens = new();
		while (!viewer.IsAtEnd) {
			var token = LexNext();
			tokens.Add(token);
		}
		tokens.Add(new(SyntaxType.EndOfFile, TextSpan.Empty, "", null));

		return new LexResult(tokens.ToImmutableArray(), diagnostics.ToImmutableArray());
	}
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
			viewer.MoveNext();
			return CreateToken(SyntaxType.Whitespace);
		}

		var diagnostic = new Diagnostic(DiagnosticLevel.Error, $"Unexpected character '{current}' at position {viewer.Position} in source.", CurrentSpan);
		diagnostics.Add(diagnostic);
		viewer.MoveNext();
		return CreateToken(SyntaxType.Unknown, diagnostic);
	}

	private SyntaxToken? GetCharacterToken(char c) {
		SyntaxType? single = c switch {
			',' => SyntaxType.CommaToken,
			':' => SyntaxType.ColonToken,
			'=' => SyntaxType.EqualsToken,
			'>' => SyntaxType.GreaterThanToken,
			'<' => SyntaxType.LesserThanToken,
			'+' => SyntaxType.PlusToken,
			'-' => SyntaxType.MinusToken,
			'*' => SyntaxType.StarToken,
			'\\' => SyntaxType.BackslashToken,
			'%' => SyntaxType.PercentToken,
			'(' => SyntaxType.OpenBraceToken,
			')' => SyntaxType.CloseBraceToken,
			'[' => SyntaxType.OpenSquareToken,
			']' => SyntaxType.CloseSquareToken,
			'{' => SyntaxType.OpenBraceToken,
			'}' => SyntaxType.CloseBraceToken,
			'\n' => SyntaxType.NewlineToken,

			_ => null
		};
		if (single is not null) {
			viewer.MoveNext();
			return CreateToken(single.Value);
		}

		switch (c) {
			case '.':
				if (viewer.Next == '.') {
					viewer.MoveDistance(2);
					return CreateToken(SyntaxType.DotDotToken);
				}
				return CreateToken(SyntaxType.DotToken);
			case '/':
				return viewer.Next == '/' ?
					GetComment() : CreateToken(SyntaxType.SlashToken);
		};

		return null;
	}

	private SyntaxToken GetNumericLiteral() {
		int literal = 0;
		while (!viewer.IsAtEnd && SyntaxRules.IsNumeric(viewer.Current))
			literal = literal * 10 + viewer.MoveNextCurrent() - '0';

		return CreateToken(SyntaxType.NumberLiteral, literal);
	}

	private SyntaxToken GetStringLiteral() {
		viewer.MoveNext();

		int startPosition = viewer.Position;
		viewer.MoveWhile(c => !SyntaxRules.IsQuote(c));
		int endPosition = viewer.Position;
		string literal = Source[startPosition..endPosition];

		viewer.MoveNext();

		return CreateToken(SyntaxType.StringLiteral, literal);
	}

	private SyntaxToken GetIdentifierOrKeywordToken() {
		string str = GetIdentifierOrKeywordString();
		var keyword = GetKeywordType(str);
		if (keyword is not null) return CreateToken(keyword.Value);
		return CreateToken(SyntaxType.Identifier, str);
	}
	private string GetIdentifierOrKeywordString() {
		int startPosition = viewer.Position;
		viewer.MoveWhile(SyntaxRules.IsKeywordValid);
		int endPosition = viewer.Position;
		return Source[startPosition..endPosition];
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

			_ => null
		};

	private SyntaxToken GetComment() {
		viewer.MoveWhile(c => c != '\n');
		return CreateToken(SyntaxType.Comment);
	}

	private SyntaxToken CreateToken(SyntaxType type) =>
		CreateToken(type, null);
	private SyntaxToken CreateToken(SyntaxType type, object? literal) =>
		new(type, CurrentSpan, Source.Substring(CurrentSpan), literal);

}
