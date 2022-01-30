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
			UnaryPlusExpressionSyntax s => BindUnaryPlus(s),
			UnaryNegationExpressionSyntax s => BindUnaryNegation(s),
			UnaryFunctionPointerExpressionSyntax s => BindUnaryFunctionPointer(s),
			UnaryLinePointerExpressionSyntax s => BindUnaryLinePointer(s),

			BinaryAdditionExpressionSyntax s => BindBinaryAddition(s),
			BinarySubtractionExpressionSyntax s => BindBinarySubtraction(s),
			BinaryMultiplicationExpressionSyntax s => BindBinaryMultiplication(s),
			BinaryDivisionExpressionSyntax s => BindBinaryDivision(s),
			BinaryModuloExpressionSyntax s => BindBinaryModulo(s),
			BinaryConcatenationExpressionSyntax s => BindBinaryConcatenation(s),

			StartExpressionSyntax s => BindStart(s),
			EndExpressionSyntax s => BindEnd(s),
			HereExpressionSyntax s => BindHere(s),

			LiteralExpressionSyntax s => BindLiteral(s),
			RangeExpressionSyntax s => BindRange(s),
			IdentifierExpressionSyntax s => BindIdentifier(s),
			FunctionInvocationExpressionSyntax s => BindFunctionInvocation(s),
			GroupingExpressionSyntax s => BindGrouping(s),

			_ => throw new InvalidOperationException()
		};

		return symbol;
	}

	private UnaryPlusExpressionSymbol BindUnaryPlus(UnaryPlusExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private UnaryNegationExpressionSymbol BindUnaryNegation(UnaryNegationExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private UnaryFunctionPointerExpressionSymbol BindUnaryFunctionPointer(UnaryFunctionPointerExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private UnaryLinePointerExpressionSymbol BindUnaryLinePointer(UnaryLinePointerExpressionSyntax syntax) {
		throw new NotImplementedException();
	}

	private BinaryAdditionExpressionSymbol BindBinaryAddition(BinaryAdditionExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private BinarySubtractionExpressionSymbol BindBinarySubtraction(BinarySubtractionExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private BinaryMultiplicationExpressionSymbol BindBinaryMultiplication(BinaryMultiplicationExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private BinaryDivisionExpressionSymbol BindBinaryDivision(BinaryDivisionExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private BinaryModuloExpressionSymbol BindBinaryModulo(BinaryModuloExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private BinaryConcatenationExpressionSymbol BindBinaryConcatenation(BinaryConcatenationExpressionSyntax syntax) {
		throw new NotImplementedException();
	}

	private StartExpressionSymbol BindStart(StartExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private EndExpressionSymbol BindEnd(EndExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private HereExpressionSymbol BindHere(HereExpressionSyntax syntax) {
		throw new NotImplementedException();
	}

	private LiteralExpressionSymbol BindLiteral(LiteralExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private RangeExpressionSymbol BindRange(RangeExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private IIdentifiableSymbol BindIdentifier(IdentifierExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private FunctionInvocationExpressionSymbol BindFunctionInvocation(FunctionInvocationExpressionSyntax syntax) {
		throw new NotImplementedException();
	}
	private GroupingExpressionSymbol BindGrouping(GroupingExpressionSyntax syntax) {
		var symbol = CreateSymbol<GroupingExpressionSymbol>(syntax);
		symbol.Expression = BindExpression(syntax.Expression);
		return symbol;
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
