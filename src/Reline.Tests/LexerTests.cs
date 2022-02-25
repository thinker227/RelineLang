using Reline.Compilation.Syntax;
using Reline.Compilation.Lexing;
using Reline.Compilation.Diagnostics;

namespace Reline.Tests;

public class LexerTests {

	[Fact]
	public void LexesRegularTokens() {
		var expectedTokens = new[] {
			Token(SyntaxType.ColonToken, 0, ":"),
			Token(SyntaxType.EqualsToken, 1, "="),
			Token(SyntaxType.LesserThanToken, 2, "<"),
			Token(SyntaxType.PlusToken, 3, "+"),
			Token(SyntaxType.MinusToken, 4, "-"),
			Token(SyntaxType.StarToken, 5, "*"),
			Token(SyntaxType.SlashToken, 6, "/"),
			Token(SyntaxType.BackslashToken, 7, "\\"),
			Token(SyntaxType.PercentToken, 8, "%"),
			Token(SyntaxType.OpenBracketToken, 9, "("),
			Token(SyntaxType.CloseBracketToken, 10, ")"),
			Token(SyntaxType.NewlineToken, 11, "\n"),
			Token(SyntaxType.DotDotToken, 12, 13, ".."),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			":=<+-*/\\%()\n..";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesKeywordTokens() {
		var expectedTokens = new[] {
			Token(SyntaxType.HereKeyword, 0, 3, "here"),
			Token(SyntaxType.StartKeyword, 5, 9, "start"),
			Token(SyntaxType.EndKeyword, 11, 13, "end"),
			Token(SyntaxType.MoveKeyword, 15, 18, "move"),
			Token(SyntaxType.SwapKeyword, 20, 23, "swap"),
			Token(SyntaxType.CopyKeyword, 25, 28, "copy"),
			Token(SyntaxType.ToKeyword, 30, 31, "to"),
			Token(SyntaxType.WithKeyword, 33, 36, "with"),
			Token(SyntaxType.ReturnKeyword, 38, 43, "return"),
			Token(SyntaxType.FunctionKeyword, 45, 52, "function"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"here start end move swap copy to with return function";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesStrings() {
		var expectedTokens = new[] {
			Token(SyntaxType.StringLiteral, 0, 17, @"""this is a string""", "this is a string"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			@"""this is a string""";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesNumbers() {
		var expectedTokens = new[] {
			Token(SyntaxType.NumberLiteral, 0, "0", 0),
			Token(SyntaxType.NumberLiteral, 2, "7", 7),
			Token(SyntaxType.NumberLiteral, 4, 8, "00123", 123),
			Token(SyntaxType.NumberLiteral, 10, 18, "123456789", 123456789),
			Token(SyntaxType.NumberLiteral, 20, 27, "10000000", 10000000),
			Token(SyntaxType.NumberLiteral, 29, 36, "00000001", 1),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"0 7 00123 123456789 10000000 00000001";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesIdentifiers() {
		var expectedTokens = new[] {
			Token(SyntaxType.Identifier, 0, 2, "foo"),
			Token(SyntaxType.Identifier, 4, 6, "bar"),
			Token(SyntaxType.Identifier, 8, 10, "Baz"),
			Token(SyntaxType.Identifier, 12, 40, "excessivelyLongIdentifierName"),
			Token(SyntaxType.Identifier, 42, 54, "_a_b_c_d_e_f_"),
			Token(SyntaxType.Identifier, 56, 57, "@i"),
			Token(SyntaxType.Identifier, 59, 68, "a123456789"),
			Token(SyntaxType.Identifier, 70, 72, "b1c"),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, ""),
		};

		string source =
			"foo bar Baz excessivelyLongIdentifierName _a_b_c_d_e_f_ @i a123456789 b1c";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);
		
		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesInvalidCharacters() {
		string source =
			"&$.[]{}";
		var lexResult = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source));

		var expectedTokens = new[] {
			Token(SyntaxType.EndOfFile, TextSpan.Empty, "").WithLeadingTrivia(new[] {
				Trivia(0, "&"),
				Trivia(1, "$"),
				Trivia(2, "."),
				Trivia(3, "["),
				Trivia(4, "]"),
				Trivia(5, "{"),
				Trivia(6, "}"),
			}),
		};
		SyntaxTokenEqualityComparer tokenComparer = new(0);
		Assert.Equal(expectedTokens, lexResult.Tokens, tokenComparer);

		var expectedDiagnostics = new[] {
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(0, 1), "", ""
			),
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(1, 2), "", ""
			),
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(2, 3), "", ""
			),
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(3, 4), "", ""
			),
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(4, 5), "", ""
			),
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(5, 6), "", ""
			),
			CompilerDiagnostics.unexpectedCharacter.ToDiagnostic(
				new(6, 7), "", ""
			),
		};
		DiagnosticEqualityComparer diagnosticComparer = new(DiagnosticComparison.IgnoreFormatting);
		Assert.Equal(expectedDiagnostics, lexResult.Diagnostics, diagnosticComparer);
	}

}
