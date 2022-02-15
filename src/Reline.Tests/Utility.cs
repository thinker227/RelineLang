using Reline.Compilation.Syntax;

namespace Reline.Tests;

public static class Utility {

	public static SyntaxToken Token(SyntaxType type, int from, int to, string text) =>
		new(type, new(from, to + 1), text, null);
	public static SyntaxToken Token(SyntaxType type, int position, string text) =>
		new(type, new(position, position + 1), text, null);
	public static SyntaxToken Token(SyntaxType type, TextSpan span, string text) =>
		new(type, span, text, null);
	public static SyntaxToken Token(SyntaxType type, int from, int to, string text, object? literal) =>
		new(type, new(from, to + 1), text, literal);
	public static SyntaxToken Token(SyntaxType type, int position, string text, object? literal) =>
		new(type, new(position, position + 1), text, literal);

}
