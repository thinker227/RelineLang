using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
using VType = Reline.Compilation.Symbols.ValueType;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Binding.Nodes;

namespace Reline.Compilation.Binding;

partial class Binder {

	/// <summary>
	/// Binds an <see cref="IExpressionSyntax"/>
	/// into an <see cref="IExpressionSymbol"/>.
	/// </summary>
	/// <param name="syntax">The syntax to bind.</param>
	/// <param name="flags">The <see cref="ExpressionBindingFlags"/>
	/// to use when binding the expression.</param>
	/// <param name="type">The expected type of the expression.</param>
	private IExpressionSymbol BindExpression(
		IExpressionSyntax syntax,
		ExpressionBindingFlags flags = ExpressionBindingFlags.None,
		VType type = VType.Any
	) {
		ExpressionBinder binder = new(flags, this);
		var expression = binder.BindExpression(syntax);

		if (type != VType.Any && !type.HasFlag(expression.GetValueType())) {
			// Implement better diagnostic later
			AddDiagnostic(expression, Diagnostics.DiagnosticLevel.Error, "Unexpected type");
		}

		return expression;
	}

}

/// <summary>
/// Binds expressions using specified <see cref="ExpressionBindingFlags"/>.
/// </summary>
internal sealed class ExpressionBinder {

	private readonly ExpressionBindingFlags flags;
	private readonly Binder binder;

	private bool None => flags.HasFlag(ExpressionBindingFlags.None);
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
		IExpressionSymbol symbol = syntax switch {
			UnaryExpressionSyntax s => BindUnary(s),
			BinaryExpressionSyntax s => BindBinary(s),
			KeywordExpressionSyntax s => BindKeyword(s),

			LiteralExpressionSyntax s => BindLiteral(s),
			IdentifierExpressionSyntax s => BindIdentifier(s),
			FunctionInvocationExpressionSyntax s => BindFunctionInvocation(s),

			_ => throw new InvalidOperationException()
		};

		return symbol;
	}

	private UnaryExpressionSymbol BindUnary(UnaryExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private BinaryExpressionSymbol BindBinary(BinaryExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private KeywordExpressionSymbol BindKeyword(KeywordExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private LiteralExpressionSymbol BindLiteral(LiteralExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private IdentifierExpressionSymbol BindIdentifier(IdentifierExpressionSyntax syntax) {
		string identifier = syntax.Identifier.Text;
		var symbol = CreateSymbol<IdentifierExpressionSymbol>(syntax);

		// Try bind labels immediately if labels are treated as constant
		if (LabelsAsConstant) {
			var labelSymbol = binder.labelBinder.GetSymbol(syntax.Identifier.Text);
			if (labelSymbol is not null) {
				symbol.Identifier = labelSymbol;
				return symbol;
			}
		}

		if (OnlyConstants) {
			AddDiagnostic(symbol, DiagnosticLevel.Error, "Labels, variables, parameters and functions may not be used in this context.");
			// BAD. No idea how to handle this case. What do you return?
			return symbol;
		}

		var identifierSymbol = binder.GetIdentifier(identifier);
		switch (identifierSymbol) {
			case null:
				AddDiagnostic(symbol, DiagnosticLevel.Error, $"Label, variable, parameter or function '{identifier}' is not declared.");
				symbol.Identifier = new MissingIdentifierSymbol { Identifier = identifier };
				break;
			case FunctionSymbol:
				AddDiagnostic(symbol, DiagnosticLevel.Error, "Functions may only be used in function pointers or function invocations. Did you intend to invoke or point to it?");
				// How to deal with functions being invalid as stand-alone identifiers?
				goto default;
			default:
				symbol.Identifier = identifierSymbol;
				break;
		}

		return symbol;
	}
	private FunctionInvocationExpressionSymbol BindFunctionInvocation(FunctionInvocationExpressionSyntax syntax) {
		throw new NotImplementedException();
	}

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
