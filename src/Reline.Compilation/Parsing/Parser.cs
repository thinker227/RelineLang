﻿using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Parsing;

public sealed class Parser {

	private readonly TokenViewer viewer;
	private readonly List<Diagnostic> diagnostics;



	public ImmutableArray<SyntaxToken> Tokens { get; }



	private Parser(ImmutableArray<SyntaxToken> tokens) {
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
		while (!viewer.IsAtEnd) {
			var line = Line();
			lines.Add(line);
		}
		return new(lines.ToImmutableArray());
	}
	private LineSyntax Line() {
		// Try find a label, otherwise the label is null
		LabelSyntax? label = null;
		if (viewer.MatchTypePattern(SyntaxType.Identifier, SyntaxType.ColonToken)) {
			var identifier = GetCurrentAdvance();
			var colonToken = GetCurrentAdvance();
			label = new(identifier, colonToken);
		}

		// Try find a statement, otherwise the statement is null
		IStatementSyntax? statement = null;
		if (!SyntaxRules.CanEndLine(viewer.Current.Type))
			statement = Statement();
		
		SyntaxToken newlineToken = Expect(SyntaxType.NewlineToken, SyntaxType.EndOfFile);

		return new(label, statement, newlineToken);
	}

	#region Statements
	private IStatementSyntax Statement() {
		// Move statement
		if (viewer.CheckType(SyntaxType.MoveKeyword))
			return MoveStatement();
		// Swap statement
		if (viewer.CheckType(SyntaxType.SwapKeyword))
			return SwapStatement();
		// Copy statement
		if (viewer.CheckType(SyntaxType.CopyKeyword))
			return CopyStatement();
		// Return statement
		if (viewer.CheckType(SyntaxType.ReturnKeyword))
			return ReturnStatement();
		// Function declaration statement
		if (viewer.CheckType(SyntaxType.FunctionKeyword))
			return FunctionDeclarationStatement();
		// Assignment statement
		if (viewer.Next.Type == SyntaxType.EqualsToken)
			return AssignmentStatement();

		// Statement is an expression statement if not any of the above
		return ExpressionStatement();
	}
	private AssignmentStatementSyntax AssignmentStatement() {
		var identifier = Expect(SyntaxType.Identifier);
		var equalsToken = GetCurrentAdvance();
		var expression = Expression();
		return new(identifier, equalsToken, expression);
	}
	private MoveStatementSyntax MoveStatement() {
		var moveKeyword = GetCurrentAdvance();
		var source = Expression();
		var toKeyword = Expect(SyntaxType.ToKeyword);
		var target = Expression();
		return new(moveKeyword, source, toKeyword, target);
	}
	private SwapStatementSyntax SwapStatement() {
		var swapKeyword = GetCurrentAdvance();
		var source = Expression();
		var withKeyword = Expect(SyntaxType.WithKeyword);
		var target = Expression();
		return new(swapKeyword, source, withKeyword, target);
	}
	private CopyStatementSyntax CopyStatement() {
		var copyKeyword = GetCurrentAdvance();
		var source = Expression();
		var toKeyword = Expect(SyntaxType.ToKeyword);
		var target = Expression();
		return new(copyKeyword, source, toKeyword, target);
	}
	private ReturnStatementSyntax ReturnStatement() {
		var returnKeyword = GetCurrentAdvance();
		var expression = Expression();
		return new(returnKeyword, expression);
	}
	private ExpressionStatementSyntax ExpressionStatement() {
		var expression = Expression();
		// Only function invocations can be used as statements
		// Make this prettier later
		if (expression is not FunctionInvocationExpressionSyntax) {
			Diagnostic diagnostic = new(DiagnosticLevel.Error, "Only function invocation expressions may be used as expression statements", expression.GetTextSpan());
			diagnostics.Add(diagnostic);
		}
		return new(expression);
	}
	
	private FunctionDeclarationStatementSyntax FunctionDeclarationStatement() {
		var functionKeyword = GetCurrentAdvance();
		var identifier = Expect(SyntaxType.Identifier);
		var body = Expression();
		ParameterListSyntax? parameterList = null;

		// Parameter list
		if (viewer.CheckType(SyntaxType.OpenBracketToken))
			parameterList = ParameterList();

		return new(functionKeyword, identifier, body, parameterList);
	}
	private ParameterListSyntax ParameterList() {
		var openBracketToken = GetCurrentAdvance();
		List<SyntaxToken> parameters = new();

		while (viewer.CheckType(SyntaxType.Identifier)) {
			var parameter = GetCurrentAdvance();
			parameters.Add(parameter);
		}

		var closeBracketToken = Expect(SyntaxType.CloseBracketToken);

		return new(openBracketToken, parameters.ToImmutableArray(), closeBracketToken);
	}
	#endregion

	#region Expressions
	private IExpressionSyntax Expression() =>
		SyntaxRules.CanBeginExpression(viewer.Current.Type) ?
		Range() : CreateInvalidExpressionTerm();
	private IExpressionSyntax Range() {
		var expression = Additive();

		// Range expression
		if (viewer.CheckType(SyntaxType.DotDotToken)) {
			var dotDotToken = GetCurrentAdvance();
			var right = Additive();
			return new RangeExpressionSyntax(expression, dotDotToken, right);
		}

		return expression;
	}
	private IExpressionSyntax Additive() {
		var expression = Multiplicative();

		// Binary addition expression
		if (viewer.CheckType(SyntaxType.PlusToken)) {
			var plusToken = GetCurrentAdvance();
			var right = Multiplicative();
			return new BinaryAdditionExpressionSyntax(expression, plusToken, right);
		}
		// Binary subtraction expression
		if (viewer.CheckType(SyntaxType.MinusToken)) {
			var minusToken = GetCurrentAdvance();
			var right = Multiplicative();
			return new BinarySubtractionExpressionSyntax(expression, minusToken, right);
		}

		return expression;
	}
	private IExpressionSyntax Multiplicative() {
		var expression = Unary();

		// Binary multiplication expression
		if (viewer.CheckType(SyntaxType.StarToken)) {
			var starToken = GetCurrentAdvance();
			var right = Unary();
			return new BinaryMultiplicationExpressionSyntax(expression, starToken, right);
		}
		// Binary division expression
		if (viewer.CheckType(SyntaxType.SlashToken)) {
			var slashToken = GetCurrentAdvance();
			var right = Unary();
			return new BinaryDivisionExpressionSyntax(expression, slashToken, right);
		}
		// Binary modulo expression
		if (viewer.CheckType(SyntaxType.PercentToken)) {
			var percentToken = GetCurrentAdvance();
			var right = Unary();
			return new BinaryModuloExpressionSyntax(expression, percentToken, right);
		}
		// Binary concatenation expression
		if (viewer.CheckType(SyntaxType.LesserThanToken)) {
			var lessThanToken = GetCurrentAdvance();
			var right = Unary();
			return new BinaryConcatenationExpressionSyntax(expression, lessThanToken, right);
		}

		return expression;
	}
	private IExpressionSyntax Unary() {
		if (viewer.CheckType(SyntaxType.PlusToken, SyntaxType.MinusToken, SyntaxType.StarToken)) {
			var op = GetCurrentAdvance();

			// Unary addition expression
			if (op.Type == SyntaxType.PlusToken)
				return new UnaryPlusExpressionSyntax(op, Primary());
			// Unary negation expression
			if (op.Type == SyntaxType.MinusToken)
				return new UnaryNegationExpressionSyntax(op, Primary());
			// Some kind of function pointer expression
			if (op.Type == SyntaxType.StarToken) {
				// Unary line pointer expression
				if (viewer.CheckType(SyntaxType.OpenSquareToken)) {
					var openSquareToken = GetCurrentAdvance();
					var expression = Primary();
					var closeSquareToken = GetCurrentAdvance();
					return new UnaryLinePointerExpressionSyntax(op, openSquareToken, expression, closeSquareToken);
				}

				// Unary line pointer expression
				var identifier = ((IdentifierExpressionSyntax)Primary()).Identifier;
				return new UnaryFunctionPointerExpressionSyntax(op, new(identifier));
			}
		}

		return Primary();
	}
	private IExpressionSyntax Primary() {
		// Here expression
		if (viewer.CheckType(SyntaxType.HereKeyword))
			return new HereExpressionSyntax(GetCurrentAdvance());
		// Start expression
		if (viewer.CheckType(SyntaxType.StartKeyword))
			return new StartExpressionSyntax(GetCurrentAdvance());
		// End expression
		if (viewer.CheckType(SyntaxType.EndKeyword))
			return new EndExpressionSyntax(GetCurrentAdvance());

		// Literal expression
		if (viewer.CheckType(SyntaxType.NumberLiteral, SyntaxType.StringLiteral))
			return new LiteralExpressionSyntax(GetCurrentAdvance());

		// Grouping expression
		if (viewer.CheckType(SyntaxType.OpenBracketToken)) {
			var openBracketToken = GetCurrentAdvance();
			var expression = Expression();
			var closeBracketToken = Expect(SyntaxType.CloseBracketToken);
			return new GroupingExpressionSyntax(openBracketToken, expression, closeBracketToken);
		}

		// Either a variable expression or a function invocation expression
		if (viewer.CheckType(SyntaxType.Identifier)) {
			// Function invocation expression
			if (viewer.Next.Type == SyntaxType.OpenBracketToken) {
				return FunctionInvocationExpression();
			}

			// Variable expression
			var identifier = GetCurrentAdvance();
			return new IdentifierExpressionSyntax(identifier);
		}

		return CreateInvalidExpressionTerm();
	}

	private FunctionInvocationExpressionSyntax FunctionInvocationExpression() {
		var identifier = GetCurrentAdvance();
		var openBracketToken = GetCurrentAdvance();

		List<IExpressionSyntax> arguments = new();
		while (!viewer.CheckType(SyntaxType.CloseBracketToken, SyntaxType.NewlineToken, SyntaxType.EndOfFile)) {
			var expression = Expression();
			arguments.Add(expression);
		}

		var closeBracketToken = Expect(SyntaxType.CloseBracketToken);

		return new(identifier, openBracketToken, arguments.ToImmutableArray(), closeBracketToken);
	}
	private IExpressionSyntax CreateInvalidExpressionTerm() {
		string diagnosticText = $"Invalid expression term '{viewer.Current.Text}'";
		var span = viewer.Current.Span;
		var token = CreateUnexpectedToken()
			.AddDiagnostic(new(DiagnosticLevel.Error, diagnosticText, span));
		return new IdentifierExpressionSyntax(token);
	}
	#endregion

	private SyntaxToken GetCurrentAdvance() {
		var trivia = viewer.GetLeadingTrivia();
		return viewer.AdvanceCurrent().WithLeadingTrivia(trivia);
	}
	private SyntaxToken Expect(params SyntaxType[] type) {
		if (viewer.CheckType(type))
			return GetCurrentAdvance();

		var token = CreateUnexpectedToken();
		return token;
	}
	private SyntaxToken CreateUnexpectedToken() =>
		CreateUnexpectedToken(viewer.Current.Span);
	private static SyntaxToken CreateUnexpectedToken(TextSpan span) =>
		new(SyntaxType.Unknown, span, string.Empty, null);

}
