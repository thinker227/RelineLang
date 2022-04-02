using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests;

public class ParserTests : ParserTestBase {

	[Fact]
	public void ParsesProgramStructure() {
		string source =
@"

";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			r.Node<LineSyntax>();
			r.Node<LineSyntax>();
			r.Node<LineSyntax>();
		}
		r.End();
	}

	[Fact]
	public void ParsesFunctionDeclarations() {
		string source =
@"function Foo 2..3 (a b c)

";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// function Foo 2..3 (a b c)
			r.Node<LineSyntax>();
			{
				// function
				r.Node<FunctionDeclarationStatementSyntax>()
					.IdentifierIs("Foo");
				// 2..3
				{
					// ..
					r.Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.DotDotToken);
					// 2
					r.Node<LiteralExpressionSyntax>()
						.ValueEquals(2);
					// 3
					r.Node<LiteralExpressionSyntax>()
						.ValueEquals(3);
				}
				// (a b c)
				r.Node<ParameterListSyntax>()
					.ParametersAre(new[] { "a", "b", "c" });
			}
			r.Node<LineSyntax>();
			r.Node<LineSyntax>();
		}
		r.End();
	}

	[Fact]
	public void ParsesSingleParameterFunctionInvocations() {
		string source = @"Write (""Hello world!"")";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// Write ("Hello world!")
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// Write ("Hello world!")
					r.Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Write");
					// ("Hello world!")
					{
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals("Hello world!");
					}
				}
			}
		}
		r.End();
	}
	[Fact]
	public void ParsesMultipleParameterFunctionInvocations() {
		string source = @"Clamp (foo 1 2)";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// Clamp (foo 1 2)
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// Clamp (foo 1 2) 
					r.Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Clamp");
					// (foo 1 2)
					{
						// foo
						r.Node<IdentifierExpressionSyntax>()
							.IdentifierIs("foo");
						// 1
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals(1);
						// 2
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals(2);
					}
				}
			}
		}
		r.End();
	}
	[Fact]
	public void ParsesMultipleExpressionParametersFunctionInvocations() {
		string source = @"Bar (1+2 (3+4)*3)";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// Bar (1+2 (3+4)*3)
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// Bar (1+2 (3+4)*3)
					r.Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Bar");
					// (1+2 (3+4)*3)
					{
						// 1+2
						{
							// +
							r.Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.PlusToken);
							// 1
							r.Node<LiteralExpressionSyntax>()
								.ValueEquals(1);
							// 2
							r.Node<LiteralExpressionSyntax>()
								.ValueEquals(2);
						}
						// (3+4)*3
						{
							// *
							r.Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.StarToken);
							// (3+4)
							{
								r.Node<GroupingExpressionSyntax>();
								// 3+4
								{
									// +
									r.Node<BinaryExpressionSyntax>()
										.OperatorTypeIs(SyntaxType.PlusToken);
									// 3
									r.Node<LiteralExpressionSyntax>()
										.ValueEquals(3);
									// 4
									r.Node<LiteralExpressionSyntax>()
										.ValueEquals(4);
								}
							}
							// 3
							r.Node<LiteralExpressionSyntax>()
								.ValueEquals(3);
						}
					}
				}
			}
		}
		r.End();
	}
	[Fact]
	public void ParsesIdentifierParametersFunctionInvocations() {
		string source = @"Baz (a B (1/2) (c) d)";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// Baz (a B (1/2) (c) d)
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// Baz (a B (1/2) (c) d)
					r.Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Baz");
					// (a B (1/2) (c) d)
					{
						// a
						r.Node<IdentifierExpressionSyntax>()
							.IdentifierIs("a");
						// B (1/2)
						{
							// B (1/2)
							r.Node<FunctionInvocationExpressionSyntax>()
								.IdentifierIs("B");
							// (1/2)
							{
								r.Node<BinaryExpressionSyntax>()
									.OperatorTypeIs(SyntaxType.SlashToken);
								r.Node<LiteralExpressionSyntax>()
									.ValueEquals(1);
								r.Node<LiteralExpressionSyntax>()
									.ValueEquals(2);
							}
						}
						// (c)
						{
							r.Node<GroupingExpressionSyntax>();
							{
								r.Node<IdentifierExpressionSyntax>()
									.IdentifierIs("c");
							}
						}
						// d
						r.Node<IdentifierExpressionSyntax>()
							.IdentifierIs("d");
					}
				}
			}
		}
		r.End();
	}

	[Fact]
	public void ParsesBinaryOperators() {
		string source =
@"1 + 2 * 3 - 4 .. 5 / 6 % 7
""a"" < ""b""";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// 1 + 2 * 3 - 4 .. 5 / 6 % 7
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// ..
					r.Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.DotDotToken);
					// 1 + 2 * 3 - 4
					{
						// -
						r.Node<BinaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.MinusToken);
						// 1 + 2 * 3
						{
							// +
							r.Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.PlusToken);
							{
								// 1
								r.Node<LiteralExpressionSyntax>()
									.ValueEquals(1);
								// 2 * 3
								{
									// *
									r.Node<BinaryExpressionSyntax>()
										.OperatorTypeIs(SyntaxType.StarToken);
									// 2
									r.Node<LiteralExpressionSyntax>()
										.ValueEquals(2);
									// 3
									r.Node<LiteralExpressionSyntax>()
										.ValueEquals(3);
								}
							}
						}
						// 4
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals(4);
					}
					// 5 / 6 % 7
					{
						// %
						r.Node<BinaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.PercentToken);
						// 5 / 6
						{
							// /
							r.Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.SlashToken);
							// 5
							r.Node<LiteralExpressionSyntax>()
								.ValueEquals(5);
							// 6
							r.Node<LiteralExpressionSyntax>()
								.ValueEquals(6);
						}
						// 7
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals(7);
					}
				}
			}
			// "a" < "b"
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// <
					r.Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.LesserThanToken);
					// "a"
					r.Node<LiteralExpressionSyntax>()
						.ValueEquals("a");
					// "b"
					r.Node<LiteralExpressionSyntax>()
						.ValueEquals("b");
				}
			}
		}
		r.End();
	}
	[Fact]
	public void ParsesUnaryOperators() {
		string source =
@"+1 + -2";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// +1 + -2
			r.Node<LineSyntax>();
			{
				r.Node<ExpressionStatementSyntax>();
				{
					// +
					r.Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.PlusToken);
					{
						// +
						r.Node<UnaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.PlusToken);
						// 1
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals(1);
					}
					{
						// -
						r.Node<UnaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.MinusToken);
						// 2
						r.Node<LiteralExpressionSyntax>()
							.ValueEquals(2);
					}
				}
			}
		}
		r.End();
	}

}
