using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Parsing;

public sealed class Parser {

	private readonly TokenViewer viewer;
	private readonly List<Diagnostic> diagnostics;



	public ImmutableArray<SyntaxToken> Tokens { get; }



	public Parser(ImmutableArray<SyntaxToken> tokens) {
		Tokens = tokens;
		viewer = new(tokens);
		diagnostics = new();
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
			try {
				var line = Line();
				lines.Add(line);
			} catch (SynchronizationException) {
				viewer.ExpectType(SyntaxType.NewlineToken);
				viewer.Advance();
			}
		}
		return new(lines.ToImmutableArray());
	}
	private LineSyntax Line() {
		LabelSyntax? label = null;
		if (viewer.MatchTypePattern(SyntaxType.Identifier, SyntaxType.ColonToken)) {
			var identifier = viewer.AdvanceCurrent();
			var colonToken = viewer.AdvanceCurrent();
			label = new(new(identifier), colonToken);
		}

		IStatementSyntax? statement = null;
		if (!viewer.MatchTypePattern(SyntaxType.NewlineToken))
			statement = Statement();

		var newlineToken = CheckType(SyntaxType.NewlineToken, SyntaxType.EndOfFile);
		viewer.Advance();

		return new(label, statement, newlineToken);
	}
	private IStatementSyntax Statement() {
		return null!;
	}

	private SyntaxToken CheckType(SyntaxType expected) {
		if (!viewer.CheckType(expected))
			CreateDiagnosticAndSynchrnoize(new[] { expected });
		return viewer.Current;
	}
	private SyntaxToken CheckType(params SyntaxType[] expected) {
		if (!viewer.CheckType(expected))
			CreateDiagnosticAndSynchrnoize(expected);
		return viewer.Current;
	}
	private void CreateDiagnosticAndSynchrnoize(SyntaxType[] expected) {
		var current = viewer.Current;
		string diagnosticText = $"Unexpected token '{current.Text}' at position {current.Span}";
		Diagnostic diagnostic = new(DiagnosticLevel.Error, diagnosticText, current.Span);
		diagnostics.Add(diagnostic);

		throw new SynchronizationException();
	}



	public class SynchronizationException : Exception { }

}
