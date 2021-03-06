using Reline.Compilation.Lexing;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Diagnostics;
using Humanizer;

namespace Reline.Compilation.Parsing;

internal sealed class Parser {

	private readonly TokenViewer viewer;
	private readonly ParserDiagnosticMap diagnostics;



	public ImmutableArray<SyntaxToken> Tokens { get; }



	private Parser(ImmutableArray<SyntaxToken> tokens) {
		Tokens = tokens;
		viewer = new(tokens);
		diagnostics = new();
	}



	public static SyntaxTree ParseString(string source) {
		var tokens = Lexer.LexSource(source);

		Parser parser = new(tokens.Tokens);
		var root = parser.Program();

		var diagnostics = tokens.Diagnostics
			.Concat(parser.diagnostics)
			.ToImmutableArray();
		return new(root, diagnostics);
	}

	private ProgramSyntax Program() {
		int currentLine = 1;
		List<LineSyntax> lines = new();
		while (!viewer.IsAtEnd) {
			var line = Line(currentLine);
			lines.Add(line);
			currentLine++;
		}
		return new(lines.ToImmutableArray());
	}
	private LineSyntax Line(int lineNumber) {
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

		var newlineToken = Expect(SyntaxType.NewlineToken, SyntaxType.EndOfFile);

		return new(lineNumber, label, statement, newlineToken);
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
			var diagnostic = CompilerDiagnostics.invalidExpressionStatement
				.ToDiagnostic(expression.GetTextSpan());
			diagnostics.AddDiagnostic(expression, diagnostic);
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
	// Praise Immo Landwerth
	// https://www.youtube.com/watch?v=3XM9vUGduhk&list=PLRAdsfhKI4OWNOSfS7EUu5GRAVmze1t2y&index=2
	private IExpressionSyntax Expression(int parentPrecedence = 0) {
		IExpressionSyntax left;

		int unaryPrecedence = viewer.Current.Type.GetUnaryOperatorPrecedence();
		if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence) {
			var operatorToken = GetCurrentAdvance();
			var operand = Expression(unaryPrecedence);
			left = new UnaryExpressionSyntax(operatorToken, operand);
		}
		else left = PrimaryExpression();
		
		while (true) {
			int binaryPrecedence = viewer.Current.Type.GetBinaryOperatorPrecedence();
			if (binaryPrecedence == 0 || binaryPrecedence <= parentPrecedence) break;

			var operatorToken = GetCurrentAdvance();
			var right = Expression(binaryPrecedence);
			left = new BinaryExpressionSyntax(left, operatorToken, right);
		}

		return left;
	}
	private IExpressionSyntax PrimaryExpression() {
		// Literal expression
		if (viewer.CheckType(SyntaxType.NumberLiteral, SyntaxType.StringLiteral))
			return new LiteralExpressionSyntax(GetCurrentAdvance());

		// Keyword expressions (here, start, end)
		if (viewer.CheckType(SyntaxType.HereKeyword, SyntaxType.StartKeyword, SyntaxType.EndKeyword))
			return new KeywordExpressionSyntax(GetCurrentAdvance());

		// Line pointer expression
		if (viewer.CheckType(SyntaxType.StarToken)) {
			var starToken = GetCurrentAdvance();
			var identifier = Expect(SyntaxType.Identifier);
			return new FunctionPointerExpressionSyntax(starToken, identifier);
		}

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
			if (viewer.Next.Type == SyntaxType.OpenBracketToken)
				return FunctionInvocationExpression();

			// Variable expression
			var identifier = GetCurrentAdvance();
			return new IdentifierExpressionSyntax(identifier);
		}

		// Bad expression
		var badToken = CreateEmptyToken(SyntaxType.Unknown);
		var diagnostic = Diagnostic.Create(
			CompilerDiagnostics.invalidExpressionTerm,
			viewer.Current.Span,
			viewer.Current.Text);
		diagnostics.AddDiagnostic(badToken, diagnostic);
		return new IdentifierExpressionSyntax(badToken);
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
	#endregion

	/// <summary>
	/// Gets the current <see cref="SyntaxToken"/> with the current leading trivia,
	/// then advances the current token.
	/// </summary>
	private SyntaxToken GetCurrentAdvance() =>
		viewer.AdvanceCurrent();
	/// <summary>
	/// Expects the current token with a specified <see cref="SyntaxType"/>.
	/// </summary>
	/// <param name="type">The <see cref="SyntaxType"/> to expect.</param>
	/// <returns>The current token if its type matches <paramref name="type"/>,
	/// otherwise a fabricated token with the type of <paramref name="type"/>
	/// with a reported diagnostic.</returns>
	private SyntaxToken Expect(SyntaxType type) {
		if (viewer.CheckType(type))
			return GetCurrentAdvance();

		var token = CreateEmptyToken(type);
		var diagnostic = Diagnostic.Create(
			CompilerDiagnostics.unexpectedToken,
			viewer.Current.Span,
			type.Humanize(LetterCasing.LowerCase),
			viewer.Current.Type.Humanize(LetterCasing.LowerCase));
		diagnostics.AddDiagnostic(token, diagnostic);
		return token;
	}
	/// <summary>
	/// Expects primary and secondary syntax types
	/// and returns the primary type if the current token does not match.
	/// </summary>
	/// <param name="primaryType">The primary type to expect.</param>
	/// <param name="secondaryTypes">The secondary types to expect.</param>
	/// <returns>The current <see cref="SyntaxToken"/> matching either
	/// <paramref name="primaryType"/> or <paramref name="secondaryTypes"/>,
	/// otherwise a fabricated token with type <paramref name="primaryType"/>.</returns>
	private SyntaxToken Expect(SyntaxType primaryType, params SyntaxType[] secondaryTypes) =>
		viewer.CheckType(secondaryTypes) ? GetCurrentAdvance() : Expect(primaryType);
	/// <summary>
	/// Creates a <see cref="SyntaxToken"/> with a specified
	/// syntax type and a text span of <see cref="TextSpan.Empty"/>.
	/// </summary>
	/// <param name="type">The <see cref="SyntaxType"/> of the token.</param>
	/// <returns>A new <see cref="SyntaxToken"/> with <paramref name="type"/>
	/// as its type and <see cref="TextSpan.Empty"/> as its text span.</returns>
	private static SyntaxToken CreateEmptyToken(SyntaxType type) =>
		new(type, TextSpan.Empty, string.Empty, null);

}
