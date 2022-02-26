using System.Linq;
using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests;

public static class SyntaxNodeExtensions {

	/// <summary>
	/// Asserts that the operator type of a
	/// <see cref="UnaryExpressionSyntax"/>
	/// is a specific operator type.
	/// </summary>
	/// <param name="syntax">The <see cref="UnaryExpressionSyntax"/>
	/// to assert.</param>
	/// <param name="type">The type to assert that the operator type is.</param>
	public static void OperatorTypeIs(this UnaryExpressionSyntax syntax, SyntaxType type) =>
		Assert.Equal(type, syntax.OperatorToken.Type);
	/// <summary>
	/// Asserts that the operator type of a
	/// <see cref="BinaryExpressionSyntax"/>
	/// is a specific operator type.
	/// </summary>
	/// <param name="syntax">The <see cref="BinaryExpressionSyntax"/>
	/// to assert.</param>
	/// <param name="type">The type to assert that the operator type is.</param>
	public static void OperatorTypeIs(this BinaryExpressionSyntax syntax, SyntaxType type) =>
		Assert.Equal(type, syntax.OperatorToken.Type);

	/// <summary>
	/// Asserts that the value of a
	/// <see cref="LiteralExpressionSyntax"/>
	/// is a specified value.
	/// </summary>
	/// <param name="syntax">The <see cref="LiteralExpressionSyntax"/>
	/// to assert.</param>
	/// <param name="value">The value to assert that the literal value is.</param>
	public static void ValueEquals(this LiteralExpressionSyntax syntax, object? value) =>
		Assert.Equal(value, syntax.Literal.Literal);

	/// <summary>
	/// Asserts that the identifiers of a
	/// <see cref="ParameterListSyntax"/>
	/// are the specified identifiers.
	/// </summary>
	/// <param name="syntax">The <see cref="ParameterListSyntax"/>
	/// to assert.</param>
	/// <param name="identifiers">The identifiers to assert that
	/// the identifiers of <paramref name="syntax"/> are.</param>
	public static void ParametersAre(this ParameterListSyntax syntax, IEnumerable<string> identifiers) =>
		Assert.Equal(identifiers, syntax.Parameters.Select(t => t.Text));

	/// <summary>
	/// Asserts that the identifier of
	/// <see cref="FunctionDeclarationStatementSyntax"/>
	/// is a specified identifier.
	/// </summary>
	/// <param name="syntax">The <see cref="FunctionDeclarationStatementSyntax"/>
	/// to assert.</param>
	/// <param name="name">The identifier to assert that
	/// the identifier of <paramref name="syntax"/> is.</param>
	public static void IdentifierIs(this FunctionDeclarationStatementSyntax syntax, string name) =>
		Assert.Equal(name, syntax.Identifier.Text);
	/// <summary>
	/// Asserts that the identifier of
	/// <see cref="FunctionInvocationExpressionSyntax"/>
	/// is a specified identifier.
	/// </summary>
	/// <param name="syntax">The <see cref="FunctionInvocationExpressionSyntax"/>
	/// to assert.</param>
	/// <param name="name">The identifier to assert that
	/// the identifier of <paramref name="syntax"/> is.</param>
	public static void IdentifierIs(this FunctionInvocationExpressionSyntax syntax, string name) =>
		Assert.Equal(name, syntax.Identifier.Text);
	/// <summary>
	/// Asserts that the identifier of
	/// <see cref="IdentifierExpressionSyntax"/>
	/// is a specified identifier.
	/// </summary>
	/// <param name="syntax">The <see cref="IdentifierExpressionSyntax"/>
	/// to assert.</param>
	/// <param name="name">The identifier to assert that
	/// the identifier of <paramref name="syntax"/> is.</param>
	public static void IdentifierIs(this IdentifierExpressionSyntax syntax, string name) =>
		Assert.Equal(name, syntax.Identifier.Text);

}
