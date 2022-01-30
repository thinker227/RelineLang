using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;
// ValueType conflicts with System.ValueType
using VType = Reline.Compilation.Symbols.ValueType;

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
		
		var symbol = CreateSymbol<LiteralExpressionSymbol>(syntax);
		symbol.Literal = 27;
		return symbol;
	}

}

/// <summary>
/// Binds expressions using specified <see cref="ExpressionBindingFlags"/>.
/// </summary>
internal sealed class ExpressionBinder {

	private readonly ExpressionBindingFlags flags;
	private readonly Binder binder;



	public ExpressionBinder(ExpressionBindingFlags flags, Binder binder) {
		this.flags = flags;
		this.binder = binder;
	}



	/// <summary>
	/// Binds an <see cref="IExpressionSyntax"/>
	/// into an <see cref="IExpressionSymbol"/>.
	/// </summary>
	public IExpressionSymbol BindExpression(IExpressionSyntax syntax) {

	}

	private TSymbol CreateSymbol<TSymbol>(ISyntaxNode syntax) where TSymbol : SymbolNode, new() =>
		binder.CreateSymbol<TSymbol>(syntax);

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
