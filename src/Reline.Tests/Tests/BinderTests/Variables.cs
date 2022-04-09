using Reline.Compilation;
using Reline.Compilation.Binding;
using Reline.Compilation.Symbols;

namespace Reline.Tests.BinderTests;

public class Variables : BinderTestBase {

	[Fact]
	public void SimpleLabelsAndVariables() {
		string source =
@"a: b = 0";
		var (r, tree) = Compile(source);

		LabelSymbol a = null!;
		VariableSymbol b = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1)
				.LabelIs(l => l
					.IdentifierIs("a")
					.Do(x => a = x));
			{
				r.Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IsType<VariableSymbol>()
						.IdentifierIs("b")
						.Do(x => b = x));
				{
					r.Node<LiteralExpressionSymbol>();
				}
			}
		}
		r.End();

		Assert.Contains(a, tree.Labels);
		Assert.Contains(b, tree.Variables);

		Assert.Empty(tree.Diagnostics);
	}
	[Fact]
	public void SimpleLabelAndVariableReferences() {
		string source =
@"a: b = 2
c: d = ""uwu""

a
b
c
d";
		var (r, tree) = Compile(source);

		LabelSymbol a = null!;
		IVariableSymbol b = null!;
		LabelSymbol c = null!;
		IVariableSymbol d = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1)
				.LabelIs(l => l
					.IdentifierIs("a")
					.Do(x => a = x));
			{
				r.Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("b")
						.Do(x => b = x));
				{
					r.Node<LiteralExpressionSymbol>()
						.HasValue(2);
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2)
				.LabelIs(l => l
					.IdentifierIs("c")
					.Do(x => c = x));
			{
				r.Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("d")
						.Do(x => d = x));
				{
					r.Node<LiteralExpressionSymbol>()
						.HasValue("uwu");
				}
			}
			r.Node<LineSymbol>();
			r.Node<LineSymbol>()
				.HasLineNumber(4);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(a));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(5);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(b));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(6);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(c));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(7);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(d));
				}
			}
		}
		r.End();

		Assert.Empty(tree.Diagnostics);
	}

}
