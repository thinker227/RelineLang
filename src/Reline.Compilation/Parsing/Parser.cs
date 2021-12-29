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

		var newlineToken = ExpectTypeAdvance(SyntaxType.NewlineToken, SyntaxType.EndOfFile);

		return new(label, statement, newlineToken);
	}

	private IStatementSyntax Statement() {
		if (viewer.CheckType(SyntaxType.MoveKeyword))
			return MoveStatement();
		if (viewer.CheckType(SyntaxType.SwapKeyword))
			return SwapStatement();
		if (viewer.CheckType(SyntaxType.CopyKeyword))
			return CopyStatement();
		if (viewer.MatchTypePattern(SyntaxType.Identifier, SyntaxType.EqualsToken))
			return AssignmentStatement();

		return ExpressionStatement();
	}
	private AssignmentStatementSyntax AssignmentStatement() {
		var identifier = viewer.AdvanceCurrent();
		var equalsToken = viewer.AdvanceCurrent();
		var expression = Expression();
		return new(new(identifier), equalsToken, expression);
	}
	private MoveStatementSyntax MoveStatement() {
		var moveKeyword = viewer.AdvanceCurrent();
		var source = Expression();
		var toKeyword = ExpectTypeAdvance(SyntaxType.ToKeyword);
		var target = Expression();
		return new(moveKeyword, source, toKeyword, target);
	}
	private SwapStatementSyntax SwapStatement() {
		var swapKeyword = viewer.AdvanceCurrent();
		var source = Expression();
		var withKeyword = ExpectTypeAdvance(SyntaxType.WithKeyword);
		var target = Expression();
		return new(swapKeyword, source, withKeyword, target);
	}
	private CopyStatementSyntax CopyStatement() {
		var copyKeyword = viewer.AdvanceCurrent();
		var source = Expression();
		var toKeyword = ExpectTypeAdvance(SyntaxType.ToKeyword);
		var target = Expression();
		return new(copyKeyword, source, toKeyword, target);
	}
	private ExpressionStatementSyntax ExpressionStatement() {
		var expression = Expression();
		if (expression is not FunctionInvocationExpressionSyntax) {
			Diagnostic diagnostic = new(DiagnosticLevel.Error, "Only function invocation expressions can be used as expression statements.", TextSpan.Empty);
			diagnostics.Add(diagnostic);
		}
		return new(expression);
	}

	private IExpressionSyntax Expression() {
		return null!;
	}

	private SyntaxToken ExpectTypeAdvance(SyntaxType expected) {
		if (!viewer.CheckType(expected))
			CreateDiagnosticAndSynchrnoize(new[] { expected });
		return viewer.AdvanceCurrent();
	}
	private SyntaxToken ExpectTypeAdvance(params SyntaxType[] expected) {
		if (!viewer.CheckType(expected))
			CreateDiagnosticAndSynchrnoize(expected);
		return viewer.AdvanceCurrent();
	}
	private void CreateDiagnosticAndSynchrnoize(params SyntaxType[] expected) {
		var current = viewer.Current;
		string diagnosticText = $"Unexpected token {current.Type} at position {current.Span}";
		Diagnostic diagnostic = new(DiagnosticLevel.Error, diagnosticText, current.Span);
		diagnostics.Add(diagnostic);

		throw new SynchronizationException();
	}



	public class SynchronizationException : Exception { }

}
