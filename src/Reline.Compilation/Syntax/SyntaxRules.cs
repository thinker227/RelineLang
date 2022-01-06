namespace Reline.Compilation.Syntax;

public static class SyntaxRules {

	public static bool IsNumeric(char c) =>
		c is >= '0' and <= '9';

	public static bool IsQuote(char c) =>
		c == '"';

	public static bool IsComment(string s) =>
		s == "//";

	public static bool IsKeywordValid(char c) =>
		c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');
	public static bool IsIdentifierValid(char c) =>
		IsKeywordValid(c) || c is '_';
	public static bool CanBeginIdentifier(char c) =>
		IsIdentifierValid(c) || c is '@';

	public static bool IsWhitespace(char c) =>
		c != '\n' && char.IsWhiteSpace(c);

	public static bool IsWhitespaceLike(SyntaxType type) =>
		type is SyntaxType.Whitespace or SyntaxType.Comment;
	public static bool CanEndLine(SyntaxType type) =>
		type is SyntaxType.NewlineToken or SyntaxType.EndOfFile;
	public static bool CanBeginStatement(SyntaxType type) =>
		type is
		SyntaxType.Identifier or
		SyntaxType.MoveKeyword or
		SyntaxType.SwapKeyword or
		SyntaxType.CopyKeyword or
		SyntaxType.ReturnKeyword or
		SyntaxType.FunctionKeyword;
	public static bool CanBeginExpression(SyntaxType type) =>
		type is
		SyntaxType.Identifier or
		SyntaxType.NumberLiteral or
		SyntaxType.StringLiteral or
		SyntaxType.HereKeyword or
		SyntaxType.StartKeyword or
		SyntaxType.EndKeyword or
		SyntaxType.OpenBracketToken or
		SyntaxType.PlusToken or
		SyntaxType.MinusToken or
		SyntaxType.StarToken;
	public static bool CanBeginType(SyntaxType type) =>
		type is
		SyntaxType.NumberKeyword or
		SyntaxType.StringKeyword;

	public static string GetTypeName(SyntaxType type) =>
		type switch {
			SyntaxType.DotToken => ".",
			SyntaxType.CommaToken => ",",
			SyntaxType.ColonToken => ":",
			SyntaxType.EqualsToken => "=",
			SyntaxType.GreaterThanToken => ">",
			SyntaxType.LesserThanToken => "<",
			SyntaxType.PlusToken => "+",
			SyntaxType.MinusToken => "-",
			SyntaxType.StarToken => "*",
			SyntaxType.SlashToken => "/",
			SyntaxType.BackslashToken => "\\",
			SyntaxType.PercentToken => "%",
			SyntaxType.OpenBracketToken => "(",
			SyntaxType.CloseBracketToken => ")",
			SyntaxType.OpenSquareToken => "[",
			SyntaxType.CloseSquareToken => "]",
			SyntaxType.OpenBraceToken => "{",
			SyntaxType.CloseBraceToken => "}",
			SyntaxType.NewlineToken => "newline",

			SyntaxType.DotDotToken => "..",

			SyntaxType.HereKeyword => "here",
			SyntaxType.StartKeyword => "start",
			SyntaxType.EndKeyword => "end",
			SyntaxType.MoveKeyword => "move",
			SyntaxType.SwapKeyword => "swap",
			SyntaxType.CopyKeyword => "copy",
			SyntaxType.ToKeyword => "to",
			SyntaxType.WithKeyword => "with",

			SyntaxType.NumberLiteral => "number literal",
			SyntaxType.StringLiteral => "string literal",
			SyntaxType.Identifier => "identifier",

			SyntaxType.Whitespace => "whitespace",
			SyntaxType.Comment => "comment",
			SyntaxType.EndOfFile => "end of file",
			SyntaxType.Unknown => "unknown",

			_ => string.Empty
		};

}
