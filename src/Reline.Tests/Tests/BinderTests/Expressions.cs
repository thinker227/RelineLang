using Reline.Compilation.Binding;
using Reline.Compilation.Symbols;
using static Reline.Tests.BinderTestUtilities;

namespace Reline.Tests.BinderTests;

public class Expressions {

	[Fact]
	public void Operators() {
		string source =
@"1 + 1
2 - 2
3 * 3
4 / 4
5 % 5
""a"" < ""b""
+7
-8";
		var (r, model) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Addition);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(2);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Subtraction);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(3);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Multiplication);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(4);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Division);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(4);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(4);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(5);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Modulo);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(5);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(5);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(6);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Concatenation);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue("a");
						r.Node<LiteralExpressionSymbol>()
							.HasValue("b");
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(7);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<UnaryExpressionSymbol>()
						.OperatorTypeIs(UnaryOperatorType.Identity);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(7);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(8);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<UnaryExpressionSymbol>()
						.OperatorTypeIs(UnaryOperatorType.Negation);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(8);
					}
				}
			}
		}
		r.End();

		Assert.Empty(model.Diagnostics);
	}

	[Fact]
	public void Keywords() {
		string source =
@"here
start
end";
		var (r, model) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<KeywordExpressionSymbol>()
						.KeywordIs(KeywordExpressionType.Here);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(2);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<KeywordExpressionSymbol>()
						.KeywordIs(KeywordExpressionType.Start);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(3);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<KeywordExpressionSymbol>()
						.KeywordIs(KeywordExpressionType.End);
				}
			}
		}
		r.End();

		Assert.Empty(model.Diagnostics);
	}

}
