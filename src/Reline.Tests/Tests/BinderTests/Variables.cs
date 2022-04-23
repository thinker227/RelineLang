using Reline.Compilation.Diagnostics;
using Reline.Compilation.Symbols;
using static Reline.Tests.BinderTestUtilities;

namespace Reline.Tests.BinderTests;

public class Variables {

	[Fact]
	public void SimpleLabelsAndVariables() {
		string source =
@"a: b = 0";
		var (r, model) = Compile(source);

		LabelSymbol a = null!;
		VariableSymbol b = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1)
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

		Assert.Contains(a, model.Labels);
		Assert.Contains(b, model.Variables);

		Assert.Empty(model.Diagnostics);
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
				.LineNumberIs(1)
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
				.LineNumberIs(2)
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
				.LineNumberIs(4);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(a));
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(5);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(b));
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(6);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(c));
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(7);
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
	[Fact]
	public void UndeclaredVariable() {
		string source =
@"x";
		var (r, tree) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BadExpressionSymbol>();
					tree.HasDiagnostic(r.Current, CompilerDiagnostics.undeclaredIdentifier);
				}
			}
		}
		r.End();
	}

}
