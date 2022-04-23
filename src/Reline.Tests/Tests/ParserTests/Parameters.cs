using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests.ParserTests;

public class Parameters : ParserTestBase {

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

}
