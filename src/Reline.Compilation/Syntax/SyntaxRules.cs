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

	public static string GetTypeSymbolOrName(this SyntaxType type) =>
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

			_ => GetTypeName(type)
		};
	public static string GetTypeName(this SyntaxType type) => type switch {
		SyntaxType.DotToken => "dot",
		SyntaxType.CommaToken => "comma",
		SyntaxType.ColonToken => "colon",
		SyntaxType.EqualsToken => "equals",
		SyntaxType.GreaterThanToken => "greater than",
		SyntaxType.LesserThanToken => "lesser than",
		SyntaxType.PlusToken => "plus",
		SyntaxType.MinusToken => "minus",
		SyntaxType.StarToken => "star",
		SyntaxType.SlashToken => "slash",
		SyntaxType.BackslashToken => "blackslash",
		SyntaxType.PercentToken => "percent",
		SyntaxType.OpenBracketToken => "open bracket",
		SyntaxType.CloseBracketToken => "close bracket",
		SyntaxType.OpenSquareToken => "open square bracket",
		SyntaxType.CloseSquareToken => "close square bracket",
		SyntaxType.OpenBraceToken => "open brace",
		SyntaxType.CloseBraceToken => " close brace",
		SyntaxType.NewlineToken => "newline",

		SyntaxType.DotDotToken => "dot dot",

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

		_ => ""
	};

	public static int GetUnaryOperatorPrecedence(this SyntaxType type) => type switch {
		SyntaxType.PlusToken or
		SyntaxType.MinusToken or
		SyntaxType.StarToken => 4,

		_ => 0
	};
	public static int GetBinaryOperatorPrecedence(this SyntaxType type) => type switch {
		SyntaxType.StarToken or
		SyntaxType.SlashToken or
		SyntaxType.PercentToken or
		SyntaxType.LesserThanToken => 3,
		
		SyntaxType.PlusToken or
		SyntaxType.MinusToken => 2,

		SyntaxType.DotDotToken => 1,

		_ => 0
	};
	public static UnaryOperatorType GetUnaryOperatorType(this SyntaxType type) => type switch {
		SyntaxType.PlusToken => UnaryOperatorType.Identity,
		SyntaxType.MinusToken => UnaryOperatorType.Negation,
		SyntaxType.StarToken => UnaryOperatorType.FunctionPointer,

		_ => throw new ArgumentException($"Syntax type '{type.GetTypeSymbolOrName()}' can not be converted to a unary operator type.", nameof(type))
	};
	public static BinaryOperatorType GetBinaryOperatorType(this SyntaxType type) => type switch {
		SyntaxType.PlusToken => BinaryOperatorType.Addition,
		SyntaxType.MinusToken => BinaryOperatorType.Subtraction,
		SyntaxType.StarToken => BinaryOperatorType.Multiplication,
		SyntaxType.SlashToken => BinaryOperatorType.Division,
		SyntaxType.PercentToken => BinaryOperatorType.Modulo,
		SyntaxType.LesserThanToken => BinaryOperatorType.Concatenation,
		SyntaxType.DotDotToken => BinaryOperatorType.Range,

		_ => throw new ArgumentException($"Syntax type '{type.GetTypeSymbolOrName()}' can not be converted to a binary operator type.", nameof(type))
	};

}
