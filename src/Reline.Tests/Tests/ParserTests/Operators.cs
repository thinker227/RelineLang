using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests.ParserTests;

public class Operators : ParserTestBase {

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
