using Reline.Compilation.Syntax;
using Reline.Compilation.Lexing;

namespace Reline.Tests;

public class LexerTests {

	[Fact]
	public void LexesRegularTokens() {
		var expectedTokens = new[] {
			Token(SyntaxType.DotToken, 0, "."),
			Token(SyntaxType.CommaToken, 1, ","),
			Token(SyntaxType.ColonToken, 2, ":"),
			Token(SyntaxType.EqualsToken, 3, "="),
			Token(SyntaxType.GreaterThanToken, 4, ">"),
			Token(SyntaxType.LesserThanToken, 5, "<"),
			Token(SyntaxType.PlusToken, 6, "+"),
			Token(SyntaxType.MinusToken, 7, "-"),
			Token(SyntaxType.StarToken, 8, "*"),
			Token(SyntaxType.SlashToken, 9, "/"),
			Token(SyntaxType.BackslashToken, 10, "\\"),
			Token(SyntaxType.PercentToken, 11, "%"),
			Token(SyntaxType.OpenBracketToken, 12, "("),
			Token(SyntaxType.CloseBracketToken, 13, ")"),
			Token(SyntaxType.OpenSquareToken, 14, "["),
			Token(SyntaxType.CloseSquareToken, 15, "]"),
			Token(SyntaxType.OpenBraceToken, 16, "{"),
			Token(SyntaxType.CloseBraceToken, 17, "}"),
			Token(SyntaxType.NewlineToken, 18, "\n"),
			Token(SyntaxType.DotDotToken, 19, 20, ".."),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			".,:=><+-*/\\%()[]{}\n..";
		var tokens = Lexer.LexSource(source).Tokens;

		Assert.Equal(expectedTokens, tokens);
	}
	[Fact]
	public void LexesKeywordTokens() {
		var expectedTokens = new[] {
			Token(SyntaxType.HereKeyword, 0, 3, "here"),
			Token(SyntaxType.Whitespace, 4, " "),
			Token(SyntaxType.StartKeyword, 5, 9, "start"),
			Token(SyntaxType.Whitespace, 10, " "),
			Token(SyntaxType.EndKeyword, 11, 13, "end"),
			Token(SyntaxType.Whitespace, 14, " "),
			Token(SyntaxType.MoveKeyword, 15, 18, "move"),
			Token(SyntaxType.Whitespace, 19, " "),
			Token(SyntaxType.SwapKeyword, 20, 23, "swap"),
			Token(SyntaxType.Whitespace, 24, " "),
			Token(SyntaxType.CopyKeyword, 25, 28, "copy"),
			Token(SyntaxType.Whitespace, 29, " "),
			Token(SyntaxType.ToKeyword, 30, 31, "to"),
			Token(SyntaxType.Whitespace, 32, " "),
			Token(SyntaxType.WithKeyword, 33, 36, "with"),
			Token(SyntaxType.Whitespace, 37, " "),
			Token(SyntaxType.ReturnKeyword, 38, 43, "return"),
			Token(SyntaxType.Whitespace, 44, " "),
			Token(SyntaxType.FunctionKeyword, 45, 52, "function"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"here start end move swap copy to with return function";
		var tokens = Lexer.LexSource(source).Tokens;

		Assert.Equal(expectedTokens, tokens);
	}
	[Fact]
	public void LexesStrings() {
		var expectedTokens = new[] {
			Token(SyntaxType.StringLiteral, 0, 17, @"""this is a string""", "this is a string"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			@"""this is a string""";
		var tokens = Lexer.LexSource(source).Tokens;

		Assert.Equal(expectedTokens, tokens);
	}
	[Fact]
	public void LexesNumbers() {
		var expectedTokens = new[] {
			Token(SyntaxType.NumberLiteral, 0, "0", 0),
			Token(SyntaxType.Whitespace, 1, " "),
			Token(SyntaxType.NumberLiteral, 2, "7", 7),
			Token(SyntaxType.Whitespace, 3, " "),
			Token(SyntaxType.NumberLiteral, 4, 8, "00123", 123),
			Token(SyntaxType.Whitespace, 9, " "),
			Token(SyntaxType.NumberLiteral, 10, 18, "123456789", 123456789),
			Token(SyntaxType.Whitespace, 19, " "),
			Token(SyntaxType.NumberLiteral, 20, 27, "10000000", 10000000),
			Token(SyntaxType.Whitespace, 28, " "),
			Token(SyntaxType.NumberLiteral, 29, 36, "00000001", 1),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"0 7 00123 123456789 10000000 00000001";
		var tokens = Lexer.LexSource(source).Tokens;

		Assert.Equal(expectedTokens, tokens);
	}
	[Fact]
	public void LexesIdentifiers() {
		var expectedTokens = new[] {
			Token(SyntaxType.Identifier, 0, 2, "foo"),
			Token(SyntaxType.Whitespace, 3, ""),
			Token(SyntaxType.Identifier, 4, 6, "bar"),
			Token(SyntaxType.Whitespace, 7, ""),
			Token(SyntaxType.Identifier, 8, 10, "Baz"),
			Token(SyntaxType.Whitespace, 11, ""),
			Token(SyntaxType.Identifier, 12, 40, "excessivelyLongIdentifierName"),
			Token(SyntaxType.Whitespace, 41, ""),
			Token(SyntaxType.Identifier, 42, 54, "_a_b_c_d_e_f_"),
			Token(SyntaxType.Whitespace, 55, ""),
			Token(SyntaxType.Identifier, 56, 57, "@i"),
			Token(SyntaxType.Whitespace, 58, ""),
			Token(SyntaxType.Identifier, 59, 68, "a123456789"),
			Token(SyntaxType.Whitespace, 69, ""),
			Token(SyntaxType.Identifier, 70, 72, "b1c"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"foo bar Baz excessivelyLongIdentifierName _a_b_c_d_e_f_ @i a123456789 b1c";
		var tokens = Lexer.LexSource(source).Tokens;

		Assert.Equal(expectedTokens, tokens);
	}
	[Fact]
	public void LexesInvalidCharacters() {
		var expectedTokens = new[] {
			Token(SyntaxType.Unknown, 0, "&"),
			Token(SyntaxType.Unknown, 1, "$"),
			Token(SyntaxType.Unknown, 2, "@"),
			Token(SyntaxType.Unknown, 3, "е"),
			Token(SyntaxType.Unknown, 4, "д"),
			Token(SyntaxType.Unknown, 5, "ц"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"&$@едц";
		var tokens = Lexer.LexSource(source).Tokens;

		Assert.Equal(expectedTokens, tokens);
	}

}
