using Reline.Compilation.Syntax.Nodes;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

partial class Binder {

	/// <summary>
	/// Binds the statement of a <see cref="LineSymbol"/>.
	/// </summary>
	private void BindLineStatement(LineSymbol symbol, IStatementSyntax statement) {
		if (statement is FunctionDeclarationStatementSyntax function) {
			// Bind function
		} else {
			symbol.Statement = BindStatement(statement);
		}
	}
	/// <summary>
	/// Binds an <see cref="IStatementSyntax"/> into an <see cref="IStatementSymbol"/>.
	/// </summary>
	private IStatementSymbol BindStatement(IStatementSyntax syntax) => syntax switch {
		ExpressionStatementSyntax s => BindExpressionStatement(s),
		AssignmentStatementSyntax s => BindAssignmentStatement(s),
		IManipulationStatementSyntax s => BindManipulationStatement(s),
		ReturnStatementSyntax s => BindReturnStatement(s),

		_ => throw new NotSupportedException($"Statement type {syntax.GetType().Name} is not supported.")
	};

	/// <summary>
	/// Binds an <see cref="ExpressionStatementSyntax"/>
	/// into an <see cref="ExpressionStatementSymbol"/>.
	/// </summary>
	private ExpressionStatementSymbol BindExpressionStatement(ExpressionStatementSyntax syntax) {
		var expression = BindExpression(syntax.Expression);
		ExpressionStatementSymbol symbol = new() {
			Syntax = syntax,
			Expression = expression
		};
		return symbol;
	}
	/// <summary>
	/// Binds an <see cref="AssignmentStatementSyntax"/>
	/// into an <see cref="AssignmentStatementSymbol"/>.
	/// </summary>
	private AssignmentStatementSymbol BindAssignmentStatement(AssignmentStatementSyntax syntax) {
		var initializer = BindExpression(syntax.Initializer);

		string identifier = syntax.Identifier.Text;
		var variable = variableBinder.GetVariable(identifier);

		AssignmentStatementSymbol symbol = new() {
			Syntax = syntax,
			Variable = variable,
			Initializer = initializer
		};
		return symbol;
	}
	/// <summary>
	/// Binds an <see cref="IManipulationStatementSyntax"/>
	/// into an <see cref="IStatementSymbol"/>.
	/// </summary>
	private IStatementSymbol BindManipulationStatement(IManipulationStatementSyntax syntax) {
		var source = BindExpression(syntax.Source);
		var target = BindExpression(syntax.Target);

		return syntax switch {
			MoveStatementSyntax => new MoveStatementSymbol {
				Syntax = syntax,
				Source = source,
				Target = target
			},
			SwapStatementSyntax => new SwapStatementSymbol {
				Syntax = syntax,
				Source = source,
				Target = target
			},
			CopyStatementSyntax => new CopyStatementSymbol {
				Syntax = syntax,
				Source = source,
				Target = target
			},

			_ => throw new NotSupportedException("Manipulation statement type {syntax.GetType().Name} is not supported.")
		};
	}
	/// <summary>
	/// Binds a <see cref="ReturnStatementSyntax"/>
	/// into a <see cref="ReturnStatementSymbol"/>.
	/// </summary>
	private ReturnStatementSymbol BindReturnStatement(ReturnStatementSyntax syntax) {
		var expression = BindExpression(syntax.Expression);

		ReturnStatementSymbol symbol = new() {
			Syntax = syntax,
			Expression = expression
		};
		return symbol;
	}
}
