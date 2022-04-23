using Reline.Compilation.Binding;
using Reline.Compilation.Symbols;
using static Reline.Tests.BinderTestUtilities;

namespace Reline.Tests.BinderTests;

public class Statements {

	[Fact]
	public void ManipulationStatements() {
		string source =
@"move 1..1 to 1
swap 2..2 with 2
copy 3..3 to 3";
		var (r, model) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<MoveStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
					r.Node<LiteralExpressionSymbol>()
						.HasValue(1);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(2);
			{
				r.Node<SwapStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
					}
					r.Node<LiteralExpressionSymbol>()
						.HasValue(2);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(3);
			{
				r.Node<CopyStatementSymbol>();
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
					}
					r.Node<LiteralExpressionSymbol>()
						.HasValue(3);
				}
			}
		}
		r.End();

		Assert.Empty(model.Diagnostics);
	}

}
