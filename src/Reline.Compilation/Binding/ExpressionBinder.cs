using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds expressions using specified <see cref="ExpressionBindingFlags"/>.
/// </summary>
internal sealed class ExpressionBinder {

	private readonly ExpressionBindingFlags flags;
	private readonly IBindingContext context;

	private bool NoVariables => flags.HasFlag(ExpressionBindingFlags.NoVariables);
	private bool NoFunctions => flags.HasFlag(ExpressionBindingFlags.NoFunctions);
	private bool LabelsAsConstant => flags.HasFlag(ExpressionBindingFlags.LabelsAsConstant);
	private bool OnlyConstants => flags.HasFlag(ExpressionBindingFlags.OnlyConstants);
	private bool ConstantsLabels => flags.HasFlag(ExpressionBindingFlags.ConstantsLabels);



	private ExpressionBinder(ExpressionBindingFlags flags, IBindingContext context) {
		this.flags = flags;
		this.context = context;
	}



	/// <summary>
	/// Binds an <see cref="IExpressionSyntax"/>
	/// into an <see cref="IExpressionSymbol"/>.
	/// </summary>
	/// <param name="syntax">The syntax to bind.</param>
	/// <param name="flags">The <see cref="ExpressionBindingFlags"/>
	/// to use to determine what is permitted in the expression.</param>
	/// <param name="context">The <see cref="IBindingContext"/> to use
	/// as the context for getting and creaing symbols and diagnostics.</param>
	/// <returns>A new <see cref="IExpressionSymbol"/> bound from
	/// <paramref name="syntax"/>.</returns>
	public static IExpressionSymbol BindExpression(
		IExpressionSyntax syntax,
		IBindingContext context,
		ExpressionBindingFlags flags = ExpressionBindingFlags.None
	) {
		ExpressionBinder expressionBinder = new(flags, context);
		var expression = expressionBinder.BindExpression(syntax);
		return expression;
	}

	/// <summary>
	/// Binds an <see cref="IExpressionSyntax"/>
	/// into an <see cref="IExpressionSymbol"/>.
	/// </summary>
	private IExpressionSymbol BindExpression(IExpressionSyntax syntax) {
		var symbol = syntax switch {
			UnaryExpressionSyntax s => BindUnary(s),
			BinaryExpressionSyntax s => BindBinary(s),
			KeywordExpressionSyntax s => BindKeyword(s),

			LiteralExpressionSyntax s => BindLiteral(s),
			GroupingExpressionSyntax s => BindGrouping(s),
			IdentifierExpressionSyntax s => BindIdentifier(s),
			FunctionInvocationExpressionSyntax s => BindFunctionInvocation(s),
			FunctionPointerExpressionSyntax s => BindFunctionPointer(s),

			_ => throw new InvalidOperationException()
		};

		return symbol;
	}

	private UnaryExpressionSymbol BindUnary(UnaryExpressionSyntax syntax) {
		var symbol = GetSymbol<UnaryExpressionSymbol>(syntax);
		symbol.OperatorType = BindUnaryOperator(syntax.OperatorToken.Type);
		symbol.Expression = BindExpression(syntax.Expression);
		return symbol;
	}
	private BinaryExpressionSymbol BindBinary(BinaryExpressionSyntax syntax) {
		var symbol = GetSymbol<BinaryExpressionSymbol>(syntax);
		symbol.OperatorType = BindBinaryOperator(syntax.OperatorToken.Type);
		symbol.Left = BindExpression(syntax.Left);
		symbol.Right = BindExpression(syntax.Right);
		return symbol;
	}
	private KeywordExpressionSymbol BindKeyword(KeywordExpressionSyntax syntax) {
		var symbol = GetSymbol<KeywordExpressionSymbol>(syntax);
		symbol.KeywordType = BindKeywordType(syntax.Keyword.Type);
		return symbol;
	}
	private LiteralExpressionSymbol BindLiteral(LiteralExpressionSyntax syntax) {
		var symbol = GetSymbol<LiteralExpressionSymbol>(syntax);
		symbol.Literal = BindLiteralValue(syntax.Literal.Literal ?? 0);
		return symbol;
	}
	private IExpressionSymbol BindGrouping(GroupingExpressionSyntax syntax) =>
		BindExpression(syntax.Expression);
	private IExpressionSymbol BindIdentifier(IdentifierExpressionSyntax syntax) {
		if (syntax.Identifier.IsMissing) {
			return BadExpression(syntax);
		}

		string identifier = syntax.Identifier.Text;

		// Try bind labels immediately if labels are treated as constant
		if (LabelsAsConstant) {
			var labelSymbol = context.GetIdentifier(syntax.Identifier.Text);
			if (labelSymbol is LabelSymbol) {
				var labelIdentifierSymbol = GetSymbol<IdentifierExpressionSymbol>(syntax);
				labelIdentifierSymbol.Identifier = labelSymbol;
				return labelIdentifierSymbol;
			}
		}

		if (OnlyConstants) {
			// Return a bad expression if non-constants are not allowed
			return BadExpression(syntax, CompilerDiagnostics.disallowedNonConstants);
		}

		var identifierSymbol = context.GetIdentifier(identifier);

		switch (identifierSymbol) {
			case null:
				return BadExpression(syntax, CompilerDiagnostics.undeclaredIdentifier, identifier);

			case FunctionSymbol:
				return BadExpression(syntax, CompilerDiagnostics.uninvokedFunction);
		}

		var symbol = GetSymbol<IdentifierExpressionSymbol>(syntax);
		symbol.Identifier = identifierSymbol;

		switch (identifierSymbol) {
			case LabelSymbol label:
				label.References.Add(symbol);
				break;
			case VariableSymbol variable:
				variable.References.Add(symbol);
				break;
			case ParameterSymbol parameter:
				parameter.References.Add(symbol);
				break;
		}

		return symbol;
	}
	private IExpressionSymbol BindFunctionInvocation(FunctionInvocationExpressionSyntax syntax) {
		if (NoFunctions) {
			// Return a bad expression if non-constants are not allowed
			return BadExpression(syntax, CompilerDiagnostics.disallowedFunctionInvocations);
		}

		if (syntax.Identifier.IsMissing) {
			return BadExpression(syntax);
		}

		string identifier = syntax.Identifier.Text;
		var identifierSymbol = context.GetIdentifier(identifier);

		List<IExpressionSymbol> arguments = new();
		foreach (var argument in syntax.Arguments)
			arguments.Add(BindExpression(argument));

		switch (identifierSymbol) {
			case null:
				AddDiagnostic(syntax.Identifier, CompilerDiagnostics.undeclaredFunction, identifier);
				return BadExpression(syntax);

			case not IFunctionSymbol:
				return BadExpression(syntax, CompilerDiagnostics.invokeNonFunction, identifier);
		}

		var symbol = GetSymbol<FunctionInvocationExpressionSymbol>(syntax);
		var function = (IFunctionSymbol)identifierSymbol;
		symbol.Function = function;
		foreach (var arg in arguments)
			symbol.Arguments.Add(arg);
		// This is kind of bad
		if (function is IDefinedIdentifiableSymbol defined) {
			foreach (var arg in arguments) defined.References.Add(arg);
		}
		return symbol;
	}
	private IExpressionSymbol BindFunctionPointer(FunctionPointerExpressionSyntax syntax) {
		// Function pointers are constant since function ranges are constant,
		// but whether they're valid or not depends on the context of the
		// expression itself and whether the context is the range of the
		// function being pointed to, a circular reference.
		// Resolving this would require additionally passing the context of
		// the expression being bound to the expression binder,
		// so as of now, function pointer expressions are not constant.

		if (OnlyConstants) {
			return BadExpression(syntax, CompilerDiagnostics.disallowedNonConstants);
		}

		if (syntax.Identifier.IsMissing) {
			return BadExpression(syntax);
		}

		string identifier = syntax.Identifier.Text;
		var identifierSymbol = context.GetIdentifier(identifier);

		switch (identifierSymbol) {
			case null:
				return BadExpression(syntax, CompilerDiagnostics.undeclaredFunction, identifier);

			case NativeFunctionSymbol:
				return BadExpression(syntax, CompilerDiagnostics.nativeFunctionPointer, identifier);

			case not FunctionSymbol:
				return BadExpression(syntax, CompilerDiagnostics.nonFunctionPointer);
		}

		var symbol = GetSymbol<FunctionPointerExpressionSymbol>(syntax);
		var function = (FunctionSymbol)identifierSymbol;
		symbol.Function = function;
		function.References.Add(function);
		return symbol;
	}

	private static UnaryOperatorType BindUnaryOperator(SyntaxType syntaxType) => syntaxType switch {
		SyntaxType.PlusToken => UnaryOperatorType.Identity,
		SyntaxType.MinusToken => UnaryOperatorType.Negation,

		_ => throw new ArgumentException($"Syntax type '{syntaxType.GetTypeSymbolOrName()}' can not be converted to a unary operator type.", nameof(syntaxType))
	};
	private static BinaryOperatorType BindBinaryOperator(SyntaxType syntaxType) => syntaxType switch {
		SyntaxType.PlusToken => BinaryOperatorType.Addition,
		SyntaxType.MinusToken => BinaryOperatorType.Subtraction,
		SyntaxType.StarToken => BinaryOperatorType.Multiplication,
		SyntaxType.SlashToken => BinaryOperatorType.Division,
		SyntaxType.PercentToken => BinaryOperatorType.Modulo,
		SyntaxType.LesserThanToken => BinaryOperatorType.Concatenation,
		SyntaxType.DotDotToken => BinaryOperatorType.Range,

		_ => throw new ArgumentException($"Syntax type '{syntaxType.GetTypeSymbolOrName()}' can not be converted to a binary operator type.", nameof(syntaxType))
	};
	private static KeywordExpressionType BindKeywordType(SyntaxType syntaxType) => syntaxType switch {
		SyntaxType.HereKeyword => KeywordExpressionType.Here,
		SyntaxType.StartKeyword => KeywordExpressionType.Start,
		SyntaxType.EndKeyword => KeywordExpressionType.End,

		_ => throw new ArgumentException($"Syntax type '{syntaxType.GetTypeSymbolOrName()}' can not be converted to a keyword type.", nameof(syntaxType))
	};
	private static BoundValue BindLiteralValue(object value) => value switch {
		int i => new(i),
		string s => new(s),
		RangeValue range => new(range),

		_ => throw new CompilationException("Unknown literal type."),
	};

	private BadExpressionSymbol BadExpression(ISyntaxNode syntax, DiagnosticDescription description, params object?[] formatArgs) {
		var symbol = GetSymbol<BadExpressionSymbol>(syntax);
		AddDiagnostic(symbol, description, formatArgs);
		return symbol;
	}
	private BadExpressionSymbol BadExpression(ISyntaxNode syntax) =>
		GetSymbol<BadExpressionSymbol>(syntax);
	private TSymbol GetSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new() =>
		context.GetSymbol<TSymbol>(syntax);
	private void AddDiagnostic(ISymbol symbol, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(symbol.Syntax?.GetTextSpan(), description, formatArgs);
	private void AddDiagnostic(ISyntaxNode node, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(node.GetTextSpan(), description, formatArgs);
	private void AddDiagnostic(SyntaxToken token, DiagnosticDescription description, params object?[] formatArgs) =>
		context.AddDiagnostic(token.Span, description, formatArgs);

}

/// <summary>
/// The binding flags for a <see cref="ExpressionBinder"/>.
/// </summary>
[Flags]
internal enum ExpressionBindingFlags {
	/// <summary>
	/// No flags are applied.
	/// </summary>
	None = 0,
	/// <summary>
	/// Variables and parameters will be treated as errors.
	/// </summary>
	NoVariables = 1,
	/// <summary>
	/// Function invocations will be treated as errors.
	/// </summary>
	NoFunctions = 2,
	/// <summary>
	/// Labels will be treated as constant values.
	/// </summary>
	LabelsAsConstant = 4,

	/// <summary>
	/// Only constant values are allowed.
	/// </summary>
	OnlyConstants = NoVariables | NoFunctions,
	/// <summary>
	/// Only constant values and labels are allowed.
	/// </summary>
	ConstantsLabels = OnlyConstants | LabelsAsConstant,
}
