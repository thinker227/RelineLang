using Reline.Compilation.Syntax;
using Reline.Compilation.Lexing;
using Reline.Compilation.Diagnostics;

namespace Reline.Tests;

public class LexerTests : TestBase {

	[Fact]
	public void LexesRegularTokens() {
		var expectedTokens = TokenSequenceBuilder.Create()
			.Add(SyntaxType.ColonToken, ":")
			.Add(SyntaxType.EqualsToken, "=")
			.Add(SyntaxType.LesserThanToken, "<")
			.Add(SyntaxType.PlusToken, "+")
			.Add(SyntaxType.MinusToken, "-")
			.Add(SyntaxType.StarToken, "*")
			.Add(SyntaxType.SlashToken, "/")
			.Add(SyntaxType.BackslashToken, "\\")
			.Add(SyntaxType.PercentToken, "%")
			.Add(SyntaxType.OpenBracketToken, "(")
			.Add(SyntaxType.CloseBracketToken, ")")
			.Add(SyntaxType.NewlineToken, "\n")
			.Add(SyntaxType.DotDotToken, "..")
			.EOF();

		string source =
			":=<+-*/\\%()\n..";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesKeywordTokens() {
		var expectedTokens = TokenSequenceBuilder.Create()
			.Add(SyntaxType.HereKeyword, "here")
			.Trivia(" ")
			.Add(SyntaxType.StartKeyword, "start")
			.Trivia(" ")
			.Add(SyntaxType.EndKeyword, "end")
			.Trivia(" ")
			.Add(SyntaxType.MoveKeyword, "move")
			.Trivia(" ")
			.Add(SyntaxType.SwapKeyword, "swap")
			.Trivia(" ")
			.Add(SyntaxType.CopyKeyword, "copy")
			.Trivia(" ")
			.Add(SyntaxType.ToKeyword, "to")
			.Trivia(" ")
			.Add(SyntaxType.WithKeyword, "with")
			.Trivia(" ")
			.Add(SyntaxType.ReturnKeyword, "return")
			.Trivia(" ")
			.Add(SyntaxType.FunctionKeyword, "function")
			.EOF();

		string source =
			"here start end move swap copy to with return function";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesStrings() {
		var expectedTokens = TokenSequenceBuilder.Create()
			.Str("this is a string")
			.EOF();

		string source =
			@"""this is a string""";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesNumbers() {
		var expectedTokens = TokenSequenceBuilder.Create()
			.Num(0)
			.Trivia(" ")
			.Num(7)
			.Trivia(" ")
			.Num("00123", 123)
			.Trivia(" ")
			.Num(123456789)
			.Trivia(" ")
			.Num(10000000)
			.Trivia(" ")
			.Num("00000001", 1)
			.EOF();

		string source =
			"0 7 00123 123456789 10000000 00000001";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		SyntaxTokenEqualityComparer comparer = new(SyntaxTokenComparison.IgnoreTrivia);
		Assert.Equal(expectedTokens, tokens, comparer);
	}
	[Fact]
	public void LexesIdentifiers() {
		var expectedTokens = TokenSequenceBuilder.Create()
			.Add(SyntaxType.Identifier, "foo")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "bar")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "Baz")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "excessivelyLongIdentifierName")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "_a_b_c_d_e_f_")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "@i")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "a123456789")
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "b1c")
			.EOF();

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

		var expectedTokens = TokenSequenceBuilder.Create()
			.Trivia("&")
			.Trivia("$")
			.Trivia(".")
			.Trivia("[")
			.Trivia("]")
			.Trivia("{")
			.Trivia("}")
			.EOF();
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
	[Fact]
	public void LexesWhitespace() {
		string source =
@" foo 
	bar		
baz
 ";
		var tokens = AssertAsync.CompletesIn(1000, () => Lexer.LexSource(source).Tokens);

		var expectedTokens = TokenSequenceBuilder.Create()
			.Trivia(" ")
			.Add(SyntaxType.Identifier, "foo")
			.Trivia(" ")
			.Trivia("\r")
			.Add(SyntaxType.NewlineToken, "\n")
			.Trivia("\t")
			.Add(SyntaxType.Identifier, "bar")
			.Trivia("\t")
			.Trivia("\t")
			.Trivia("\r")
			.Add(SyntaxType.NewlineToken, "\n")
			.Add(SyntaxType.Identifier, "baz")
			.Trivia("\r")
			.Add(SyntaxType.NewlineToken, "\n")
			.Trivia(" ")
			.EOF();

		/*var expectedTokens = new[] {
			Token(SyntaxType.Identifier, 1, 3, "foo").WithLeadingTrivia(new[] {
				Trivia(0, " ")
			}),
			Token(SyntaxType.NewlineToken, 6, "\n").WithLeadingTrivia(new[] {
				Trivia(4, " "),
				Trivia(5, "\r"),
			}),
			Token(SyntaxType.Identifier, 8, 10, "bar").WithLeadingTrivia(new[] {
				Trivia(7, "\t"),
			}),
			Token(SyntaxType.NewlineToken, 14, "\n").WithLeadingTrivia(new[] {
				Trivia(11, "\t"),
				Trivia(12, "\t"),
				Trivia(13, "\r"),
			}),
			Token(SyntaxType.Identifier, 15, 17, "baz"),
			Token(SyntaxType.NewlineToken, 19, "\n").WithLeadingTrivia(new[] {
				Trivia(18, "\r"),
			}),
			Token(SyntaxType.EndOfFile, TextSpan.Empty, "").WithLeadingTrivia(new[] {
				Trivia(20, " "),
			}),
		};*/
		SyntaxTokenEqualityComparer comparer = new(0);
		Assert.Equal(expectedTokens, tokens, comparer);
	}

}
