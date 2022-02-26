using Reline.Compilation.Syntax;

namespace Reline.Tests;

public abstract class TestBase {

	protected static SyntaxToken Token(SyntaxType type, int from, int to, string text) =>
		new(type, new(from, to + 1), text, null);
	protected static SyntaxToken Token(SyntaxType type, int position, string text) =>
		new(type, new(position, position + 1), text, null);
	protected static SyntaxToken Token(SyntaxType type, TextSpan span, string text) =>
		new(type, span, text, null);
	protected static SyntaxToken Token(SyntaxType type, int from, int to, string text, object? literal) =>
		new(type, new(from, to + 1), text, literal);
	protected static SyntaxToken Token(SyntaxType type, int position, string text, object? literal) =>
		new(type, new(position, position + 1), text, literal);

	protected static SyntaxTrivia Trivia(int position, string text) =>
		new(new(position, position + 1), text);
	protected static SyntaxTrivia Trivia(TextSpan span, string text) =>
		new(span, text);

}
