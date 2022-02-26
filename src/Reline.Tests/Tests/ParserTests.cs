using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests;

public class ParserTests : ParserTestBase {

	[Fact]
	public void ParsesProgramStructure() {
		string source =
@"

";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			Node<LineSyntax>();
			Node<LineSyntax>();
			Node<LineSyntax>();
		}
		End();
	}

	[Fact]
	public void ParsesFunctionDeclarations() {
		string source =
@"function Foo 2..3 (a b c)

";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// function Foo 2..3 (a b c)
			Node<LineSyntax>();
			{
				// function
				Node<FunctionDeclarationStatementSyntax>()
					.IdentifierIs("Foo");
				// 2..3
				{
					// ..
					Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.DotDotToken);
					// 2
					Node<LiteralExpressionSyntax>()
						.ValueEquals(2);
					// 3
					Node<LiteralExpressionSyntax>()
						.ValueEquals(3);
				}
				// (a b c)
				Node<ParameterListSyntax>()
					.ParametersAre(new[] { "a", "b", "c" });
			}
			Node<LineSyntax>();
			Node<LineSyntax>();
		}
		End();
	}

	[Fact]
	public void ParsesSingleParameterFunctionInvocations() {
		string source = @"Write (""Hello world!"")";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// Write ("Hello world!")
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// Write ("Hello world!")
					Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Write");
					// ("Hello world!")
					{
						Node<LiteralExpressionSyntax>()
							.ValueEquals("Hello world!");
					}
				}
			}
		}
		End();
	}
	[Fact]
	public void ParsesMultipleParameterFunctionInvocations() {
		string source = @"Clamp (foo 1 2)";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// Clamp (foo 1 2)
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// Clamp (foo 1 2) 
					Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Clamp");
					// (foo 1 2)
					{
						// foo
						Node<IdentifierExpressionSyntax>()
							.IdentifierIs("foo");
						// 1
						Node<LiteralExpressionSyntax>()
							.ValueEquals(1);
						// 2
						Node<LiteralExpressionSyntax>()
							.ValueEquals(2);
					}
				}
			}
		}
		End();
	}
	[Fact]
	public void ParsesMultipleExpressionParametersFunctionInvocations() {
		string source = @"Bar (1+2 (3+4)*3)";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// Bar (1+2 (3+4)*3)
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// Bar (1+2 (3+4)*3)
					Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Bar");
					// (1+2 (3+4)*3)
					{
						// 1+2
						{
							// +
							Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.PlusToken);
							// 1
							Node<LiteralExpressionSyntax>()
								.ValueEquals(1);
							// 2
							Node<LiteralExpressionSyntax>()
								.ValueEquals(2);
						}
						// (3+4)*3
						{
							// *
							Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.StarToken);
							// (3+4)
							{
								Node<GroupingExpressionSyntax>();
								// 3+4
								{
									// +
									Node<BinaryExpressionSyntax>()
										.OperatorTypeIs(SyntaxType.PlusToken);
									// 3
									Node<LiteralExpressionSyntax>()
										.ValueEquals(3);
									// 4
									Node<LiteralExpressionSyntax>()
										.ValueEquals(4);
								}
							}
							// 3
							Node<LiteralExpressionSyntax>()
								.ValueEquals(3);
						}
					}
				}
			}
		}
		End();
	}
	[Fact]
	public void ParsesIdentifierParametersFunctionInvocations() {
		string source = @"Baz (a B (1/2) (c) d)";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// Baz (a B (1/2) (c) d)
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// Baz (a B (1/2) (c) d)
					Node<FunctionInvocationExpressionSyntax>()
						.IdentifierIs("Baz");
					// (a B (1/2) (c) d)
					{
						// a
						Node<IdentifierExpressionSyntax>()
							.IdentifierIs("a");
						// B (1/2)
						{
							// B (1/2)
							Node<FunctionInvocationExpressionSyntax>()
								.IdentifierIs("B");
							// (1/2)
							{
								Node<BinaryExpressionSyntax>()
									.OperatorTypeIs(SyntaxType.SlashToken);
								Node<LiteralExpressionSyntax>()
									.ValueEquals(1);
								Node<LiteralExpressionSyntax>()
									.ValueEquals(2);
							}
						}
						// (c)
						{
							Node<GroupingExpressionSyntax>();
							{
								Node<IdentifierExpressionSyntax>()
									.IdentifierIs("c");
							}
						}
						// d
						Node<IdentifierExpressionSyntax>()
							.IdentifierIs("d");
					}
				}
			}
		}
		End();
	}

	[Fact]
	public void ParsesBinaryOperators() {
		string source =
@"1 + 2 * 3 - 4 .. 5 / 6 % 7
""a"" < ""b""";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// 1 + 2 * 3 - 4 .. 5 / 6 % 7
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// ..
					Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.DotDotToken);
					// 1 + 2 * 3 - 4
					{
						// -
						Node<BinaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.MinusToken);
						// 1 + 2 * 3
						{
							// +
							Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.PlusToken);
							{
								// 1
								Node<LiteralExpressionSyntax>()
									.ValueEquals(1);
								// 2 * 3
								{
									// *
									Node<BinaryExpressionSyntax>()
										.OperatorTypeIs(SyntaxType.StarToken);
									// 2
									Node<LiteralExpressionSyntax>()
										.ValueEquals(2);
									// 3
									Node<LiteralExpressionSyntax>()
										.ValueEquals(3);
								}
							}
						}
						// 4
						Node<LiteralExpressionSyntax>()
							.ValueEquals(4);
					}
					// 5 / 6 % 7
					{
						// %
						Node<BinaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.PercentToken);
						// 5 / 6
						{
							// /
							Node<BinaryExpressionSyntax>()
								.OperatorTypeIs(SyntaxType.SlashToken);
							// 5
							Node<LiteralExpressionSyntax>()
								.ValueEquals(5);
							// 6
							Node<LiteralExpressionSyntax>()
								.ValueEquals(6);
						}
						// 7
						Node<LiteralExpressionSyntax>()
							.ValueEquals(7);
					}
				}
			}
			// "a" < "b"
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// <
					Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.LesserThanToken);
					// "a"
					Node<LiteralExpressionSyntax>()
						.ValueEquals("a");
					// "b"
					Node<LiteralExpressionSyntax>()
						.ValueEquals("b");
				}
			}
		}
		End();
	}
	[Fact]
	public void ParsesUnaryOperators() {
		string source =
@"+1 + -2";
		SetTree(source);

		Node<ProgramSyntax>();
		{
			// +1 + -2
			Node<LineSyntax>();
			{
				Node<ExpressionStatementSyntax>();
				{
					// +
					Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.PlusToken);
					{
						// +
						Node<UnaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.PlusToken);
						// 1
						Node<LiteralExpressionSyntax>()
							.ValueEquals(1);
					}
					{
						// -
						Node<UnaryExpressionSyntax>()
							.OperatorTypeIs(SyntaxType.MinusToken);
						// 2
						Node<LiteralExpressionSyntax>()
							.ValueEquals(2);
					}
				}
			}
		}
		End();
	}

}
