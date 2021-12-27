using Reline.Compilation.Syntax;

namespace Reline.Compilation.Lexing;

public sealed class Lexer {

	private readonly SourceViewer viewer;
	private int lexemeStartPosition;



	private TextSpan CurrentSpan =>
		new(lexemeStartPosition, viewer.Position);
	public string Source { get; }



	public Lexer(string source) {
		viewer = new(source);
		Source = source;
	}



	public ImmutableArray<SyntaxToken> LexAll() {
		List<SyntaxToken> tokens = new();
		while (!viewer.IsAtEnd) {
			var token = LexNext();
			tokens.Add(token);
		}
		return tokens.ToImmutableArray();
	}

	public SyntaxToken LexNext() {
		lexemeStartPosition = viewer.Position;
		char current = viewer.Current;

		var single = GetSingleCharacterType(current);
		if (single is not null) {
			viewer.MoveNext();
			return CreateToken(single.Value);
		}

		if (SyntaxRules.IsQuote(current)) return GetStringLiteral();
		if (SyntaxRules.IsNumeric(current)) return GetNumericLiteral();
		if (SyntaxRules.CanBeginIdentifier(current)) return GetIdentifierOrKeywordToken();
		if (SyntaxRules.IsWhitespace(current)) {
			viewer.MoveNext();
			return CreateToken(SyntaxType.Whitespace);
		}

		throw new Exception($"Unexpected character '{current}' at position {viewer.Position}.");
	}

	private static SyntaxType? GetSingleCharacterType(char c) =>
		c switch {
			'.' => SyntaxType.DotToken,
			',' => SyntaxType.CommaToken,
			':' => SyntaxType.ColonToken,
			'=' => SyntaxType.EqualsToken,
			'>' => SyntaxType.GreaterThanToken,
			'<' => SyntaxType.LesserThanToken,
			'+' => SyntaxType.PlusToken,
			'-' => SyntaxType.MinusToken,
			'*' => SyntaxType.StarToken,
			'/' => SyntaxType.SlashToken,
			'\\' => SyntaxType.BackslashToken,
			'%' => SyntaxType.PercentToken,
			'(' => SyntaxType.OpenBraceToken,
			')' => SyntaxType.CloseBraceToken,
			'[' => SyntaxType.OpenSquareToken,
			']' => SyntaxType.CloseSquareToken,
			'{' => SyntaxType.OpenBraceToken,
			'}' => SyntaxType.CloseBraceToken,
			'\n' => SyntaxType.Newline,

			_ => null
		};

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

	private SyntaxToken CreateToken(SyntaxType type) =>
		CreateToken(type, null);
	private SyntaxToken CreateToken(SyntaxType type, object? literal) =>
		new(type, CurrentSpan, Source.Substring(CurrentSpan), literal);

}
