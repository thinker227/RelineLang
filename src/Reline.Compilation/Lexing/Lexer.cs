using Reline.Compilation.Syntax;

namespace Reline.Compilation.Lexing;

public sealed class Lexer {

	private readonly SourceViewer viewer;



	public string Source { get; }



	public Lexer(string source) {
		viewer = new(source);
		Source = source;
	}



	public ImmutableArray<SyntaxToken> LexAll() {
	}

	public SyntaxToken LexNext() {
	}

}
