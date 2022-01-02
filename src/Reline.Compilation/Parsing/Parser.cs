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

		return new(program);
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
		if (!SyntaxRules.CanEndLine(viewer.Current.Type))
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
			Diagnostic diagnostic = new(DiagnosticLevel.Error, "Only function invocation expressions may be used as expression statements", expression.Span);
			diagnostics.Add(diagnostic);
		}
		return new(expression);
	}

	private IExpressionSyntax Expression() =>
		Range();
	private IExpressionSyntax Range() {
		var expression = Additive();

		if (viewer.CheckType(SyntaxType.DotDotToken)) {
			var dotDotToken = viewer.AdvanceCurrent();
			var right = Additive();
			return new RangeExpressionSyntax(expression, dotDotToken, right);
		}

		return expression;
	}
	private IExpressionSyntax Additive() {
		var expression = Multiplicative();

		if (viewer.CheckType(SyntaxType.PlusToken)) {
			var plusToken = viewer.AdvanceCurrent();
			var right = Multiplicative();
			return new BinaryAdditionExpressionSyntax(expression, plusToken, right);
		}
		if (viewer.CheckType(SyntaxType.MinusToken)) {
			var minusToken = viewer.AdvanceCurrent();
			var right = Multiplicative();
			return new BinarySubtractionExpressionSyntax(expression, minusToken, right);
		}

		return expression;
	}
	private IExpressionSyntax Multiplicative() {
		var expression = Unary();

		if (viewer.CheckType(SyntaxType.StarToken)) {
			var starToken = viewer.AdvanceCurrent();
			var right = Unary();
			return new BinaryMultiplicationExpressionSyntax(expression, starToken, right);
		}
		if (viewer.CheckType(SyntaxType.SlashToken)) {
			var slashToken = viewer.AdvanceCurrent();
			var right = Unary();
			return new BinaryDivisionExpressionSyntax(expression, slashToken, right);
		}
		if (viewer.CheckType(SyntaxType.PercentToken)) {
			var percentToken = viewer.AdvanceCurrent();
			var right = Unary();
			return new BinaryModuloExpressionSyntax(expression, percentToken, right);
		}
		if (viewer.CheckType(SyntaxType.LesserThanToken)) {
			var lessThanToken = viewer.AdvanceCurrent();
			var right = Unary();
			return new BinaryConcatenationExpressionSyntax(expression, lessThanToken, right);
		}

		return expression;
	}
	private IExpressionSyntax Unary() {
		if (viewer.CheckType(SyntaxType.PlusToken, SyntaxType.MinusToken, SyntaxType.StarToken)) {
			var op = viewer.AdvanceCurrent();

			if (op.Type == SyntaxType.PlusToken)
				return new UnaryPlusExpressionSyntax(op, Primary());
			if (op.Type == SyntaxType.MinusToken)
				return new UnaryNegationExpressionSyntax(op, Primary());
			if (op.Type == SyntaxType.StarToken) {
				if (viewer.CheckType(SyntaxType.OpenSquareToken)) {
					var openSquareToken = viewer.AdvanceCurrent();
					var expression = Primary();
					var closeSquareToken = viewer.AdvanceCurrent();
					return new UnaryLinePointerExpressionSyntax(op, openSquareToken, expression, closeSquareToken);
				}

				var identifier = ((VariableExpressionSyntax)Primary()).Identifier;
				return new UnaryFunctionPointerExpressionSyntax(op, identifier);
			}
		}

		return Primary();
	}
	private IExpressionSyntax Primary() {
		if (viewer.CheckType(SyntaxType.HereKeyword))
			return new HereExpressionSyntax(viewer.AdvanceCurrent());
		if (viewer.CheckType(SyntaxType.StartKeyword))
			return new StartExpressionSyntax(viewer.AdvanceCurrent());
		if (viewer.CheckType(SyntaxType.EndKeyword))
			return new EndExpressionSyntax(viewer.AdvanceCurrent());

		if (viewer.CheckType(SyntaxType.NumberLiteral, SyntaxType.StringLiteral))
			return new LiteralExpressionSyntax(viewer.AdvanceCurrent());

		if (viewer.CheckType(SyntaxType.OpenBracketToken)) {
			var openBracketToken = viewer.AdvanceCurrent();
			var expression = Expression();
			var closeBracketToken = ExpectTypeAdvance(SyntaxType.CloseBracketToken);
			return new GroupingExpressionSyntax(openBracketToken, expression, closeBracketToken);
		}

		// Either a variable expression or a function invocation expression
		if (viewer.CheckType(SyntaxType.Identifier)) {
			var identifier = viewer.AdvanceCurrent();

			// Function invocation expression
			if (viewer.CheckType(SyntaxType.OpenBracketToken))
				return FunctionInvocationExpression();

			// Variable expression
			return new VariableExpressionSyntax(new(identifier));
		}

		CreateDiagnosticAndSynchrnoize();
		return null!; // Will never be executed since CreateDiagnosticAndSynchrnoize throws an exception.
	}

	private FunctionInvocationExpressionSyntax FunctionInvocationExpression() {
		var identifier = viewer.Previous;
		var openBracketToken = viewer.AdvanceCurrent();

		List<IExpressionSyntax> arguments = new();
		while (true) {
			if (viewer.CheckType(SyntaxType.CloseBracketToken)) break;
			var expression = Expression();
			arguments.Add(expression);
		}

		var closeBracketToken = ExpectTypeAdvance(SyntaxType.CloseBracketToken);

		return new(new(identifier), openBracketToken, arguments.ToImmutableArray(), closeBracketToken);
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
