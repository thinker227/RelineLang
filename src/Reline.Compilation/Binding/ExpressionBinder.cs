using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using VType = Reline.Compilation.Symbols.ValueType;
using Reline.Compilation.Diagnostics;

namespace Reline.Compilation.Binding;

partial class Binder {

	/// <summary>
	/// Binds an <see cref="IExpressionSyntax"/>
	/// into an <see cref="IExpressionSymbol"/>.
	/// </summary>
	/// <param name="syntax">The syntax to bind.</param>
	/// <param name="flags">The <see cref="ExpressionBindingFlags"/>
	/// to use when binding the expression.</param>
	private IExpressionSymbol BindExpression(
		IExpressionSyntax syntax,
		ExpressionBindingFlags flags = ExpressionBindingFlags.None
	) {
		ExpressionBinder binder = new(flags, this);
		var expression = binder.BindExpression(syntax);
		return expression;
	}

}

/// <summary>
/// Binds expressions using specified <see cref="ExpressionBindingFlags"/>.
/// </summary>
internal sealed class ExpressionBinder {

	private readonly ExpressionBindingFlags flags;
	private readonly Binder binder;

	private bool NoVariables => flags.HasFlag(ExpressionBindingFlags.NoVariables);
	private bool NoFunctions => flags.HasFlag(ExpressionBindingFlags.NoFunctions);
	private bool LabelsAsConstant => flags.HasFlag(ExpressionBindingFlags.LabelsAsConstant);
	private bool OnlyConstants => flags.HasFlag(ExpressionBindingFlags.OnlyConstants);
	private bool ConstantsLabels => flags.HasFlag(ExpressionBindingFlags.ConstantsLabels);



	public ExpressionBinder(ExpressionBindingFlags flags, Binder binder) {
		this.flags = flags;
		this.binder = binder;
	}



	/// <summary>
	/// Binds an <see cref="IExpressionSyntax"/>
	/// into an <see cref="IExpressionSymbol"/>.
	/// </summary>
	public IExpressionSymbol BindExpression(IExpressionSyntax syntax) {
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
		var symbol = CreateSymbol<UnaryExpressionSymbol>(syntax);
		symbol.OperatorType = BindUnaryOperator(syntax.OperatorToken.Type);
		symbol.Expression = BindExpression(syntax.Expression);
		return symbol;
	}
	private BinaryExpressionSymbol BindBinary(BinaryExpressionSyntax syntax) {
		var symbol = CreateSymbol<BinaryExpressionSymbol>(syntax);
		symbol.OperatorType = BindBinaryOperator(syntax.OperatorToken.Type);
		symbol.Left = BindExpression(syntax.Left);
		symbol.Right = BindExpression(syntax.Right);
		return symbol;
	}
	private KeywordExpressionSymbol BindKeyword(KeywordExpressionSyntax syntax) {
		var symbol = CreateSymbol<KeywordExpressionSymbol>(syntax);
		symbol.KeywordType = BindKeywordType(syntax.Keyword.Type);
		return symbol;
	}
	private LiteralExpressionSymbol BindLiteral(LiteralExpressionSyntax syntax) {
		var symbol = CreateSymbol<LiteralExpressionSymbol>(syntax);
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
			var labelSymbol = binder.LabelBinder.GetSymbol(syntax.Identifier.Text);
			if (labelSymbol is not null) {
				var labelIdentifierSymbol = CreateSymbol<IdentifierExpressionSymbol>(syntax);
				labelIdentifierSymbol.Identifier = labelSymbol;
				return labelIdentifierSymbol;
			}
		}

		if (OnlyConstants) {
			// Return a bad expression if non-constants are not allowed
			return BadExpression(syntax, "Labels, variables, parameters and functions may not be used in this context.");
		}

		var identifierSymbol = binder.GetIdentifier(identifier);

		switch (identifierSymbol) {
			case null:
				return BadExpression(syntax, $"Label, variable, parameter or function '{identifier}' is not declared.");

			case FunctionSymbol:
				return BadExpression(syntax, "Functions may only be used in function pointers or function invocations. Did you intend to invoke or point to it?");
		}

		var symbol = CreateSymbol<IdentifierExpressionSymbol>(syntax);
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
			return BadExpression(syntax, "Functions may not be used in this context.");
		}

		if (syntax.Identifier.IsMissing) {
			return BadExpression(syntax);
		}

		string identifier = syntax.Identifier.Text;
		var identifierSymbol = binder.GetIdentifier(identifier);

		switch (identifierSymbol) {
			case null:
				return BadExpression(syntax, $"Function '{identifier}' is not declared.");

			case not FunctionSymbol:
				return BadExpression(syntax, $"Cannot invoke label, variable or parameter '{identifier}'.");
		}

		var symbol = CreateSymbol<FunctionInvocationExpressionSymbol>(syntax);
		var function = (FunctionSymbol)identifierSymbol;
		symbol.Function = function;
		function.References.Add(symbol);
		foreach (var argument in syntax.Arguments)
			symbol.Arguments.Add(BindExpression(argument));
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
			return BadExpression(syntax, "Function pointers may not be used in this context.");
		}

		if (syntax.Identifier.IsMissing) {
			return BadExpression(syntax);
		}

		string identifier = syntax.Identifier.Text;
		var identifierSymbol = binder.GetIdentifier(identifier);

		switch (identifierSymbol) {
			case null:
				return BadExpression(syntax, $"Function '{identifier}' is not declared.");

			case not FunctionSymbol:
				return BadExpression(syntax, $"Cannot point to label, variable or parameter '{identifier}'.");
		}

		var symbol = CreateSymbol<FunctionPointerExpressionSymbol>(syntax);
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
	private static LiteralValue BindLiteralValue(object value) => value switch {
		int i => new(i),
		string s => new(s),
		RangeLiteral range => new(range),

		_ => throw new CompilationException("Unknown literal type."),
	};

	private BadExpressionSymbol BadExpression(ISyntaxNode syntax, string diagnosticDescription) {
		var symbol = CreateSymbol<BadExpressionSymbol>(syntax);
		AddDiagnostic(symbol, DiagnosticLevel.Error, diagnosticDescription);
		return symbol;
	}
	private BadExpressionSymbol BadExpression(ISyntaxNode syntax) =>
		CreateSymbol<BadExpressionSymbol>(syntax);
	private TSymbol CreateSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new() =>
		binder.CreateSymbol<TSymbol>(syntax);
	private void AddDiagnostic(ISymbol symbol, DiagnosticLevel level, string description) =>
		binder.AddDiagnostic(symbol, level, description);

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
