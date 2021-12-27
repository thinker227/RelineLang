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

}
