using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Parsing;

public sealed class Parser {

	private readonly TokenViewer viewer;



	public ImmutableArray<SyntaxToken> Tokens { get; }



	public Parser(ImmutableArray<SyntaxToken> tokens) {
		Tokens = tokens;
		viewer = new(tokens);
	}



	public static IOperationResult<SyntaxTree> ParseTokens(IOperationResult<ImmutableArray<SyntaxToken>> tokens) {
		if (tokens.HasErrors()) throw new CompilationException("Cannot parse invalid tokens.");

		Parser parser = new(tokens.Result);
		var result = parser.ParseAll();
		return new ParseResult(ImmutableArray.Create<Diagnostic>(), result);
	}
	private SyntaxTree ParseAll() {
		var program = Program();

		return new(null!);
	}

	private ProgramSyntax Program() {
		List<LineSyntax> lines = new();
		while (viewer.Current.Type != SyntaxType.EndOfFile) {
			var line = Line();
			lines.Add(line);
		}
		return new(lines.ToImmutableArray());
	}
	private LineSyntax Line() {
		viewer.ExpectNotWhitespace();

		LabelSyntax? label = null;
		if (viewer.MatchTypePatternNotWhitespace(SyntaxType.Identifier, SyntaxType.ColonToken)) {
			var identifier = viewer.Current;
			viewer.AdvanceNotWhitespace();
			var colonToken = viewer.Current;
			viewer.AdvanceNotWhitespace();
			label = new(new(identifier), colonToken);
		}

		IStatementSyntax? statement = null;
		if (!viewer.MatchTypePatternNotWhitespace(SyntaxType.NewlineToken)) {

		}

		var newlineToken = viewer.AdvanceCurrent();

		return new(label, statement, newlineToken);
	}

}
