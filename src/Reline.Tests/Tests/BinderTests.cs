using Reline.Compilation.Symbols;

namespace Reline.Tests;

public class BinderTests : BinderTestBase {

	[Fact]
	public void LineNumbers() {
		const int lines = 200;

		string source = new('\n', lines - 1);
		var tree = SetTree(source);

		Node<ProgramSymbol>();
		{
			for (int i = 0; i < lines; i++) {
				Node<LineSymbol>()
					.HasLineNumber(i + 1);
			}
		}
		End();

		Assert.Empty(tree.Diagnostics);
	}

	[Fact]
	public void ExpressionStatementFunctionInvocation() {
		string source =
@"Write (""Hello world!"")";
		var tree = SetTree(source);

		Node<ProgramSymbol>();
		{
			Node<LineSymbol>()
				.HasLineNumber(1);
			{
				Node<ExpressionStatementSymbol>();
				{
					Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IdentifierIs("Write"));
					{
						Node<LiteralExpressionSymbol>();
					}
				}
			}
		}
		End();

		Assert.Empty(tree.Diagnostics);
	}

	[Fact]
	public void SimpleLabelsAndVariables() {
		string source =
@"a: b = 0";
		var tree = SetTree(source);

		Node<ProgramSymbol>();
		{
			Node<LineSymbol>()
				.HasLineNumber(1)
				.LabelIs(l => l
					.IdentifierIs("a"));
			{
				Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("b"));
				{
					Node<LiteralExpressionSymbol>();
				}
			}
		}
		End();
		
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
		var tree = SetTree(source);

		LabelSymbol a = null!;
		IVariableSymbol b = null!;
		LabelSymbol c = null!;
		IVariableSymbol d = null!;

		Node<ProgramSymbol>();
		{
			Node<LineSymbol>()
				.HasLineNumber(1)
				.LabelIs(l => l
					.IdentifierIs("a")
					.Do(() => a = l));
			{
				Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("b")
						.Do(() => b = v));
				{
					Node<LiteralExpressionSymbol>()
						.HasValue(2);
				}
			}
			Node<LineSymbol>()
				.HasLineNumber(2)
				.LabelIs(l => l
					.IdentifierIs("c")
					.Do(() => c = l));
			{
				Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("d")
						.Do(() => d = v));
				{
					Node<LiteralExpressionSymbol>()
						.HasValue("uwu");
				}
			}
			Node<LineSymbol>();
			Node<LineSymbol>()
				.HasLineNumber(4);
			{
				Node<ExpressionStatementSymbol>();
				{
					Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.Is(a));
				}
			}
			Node<LineSymbol>()
				.HasLineNumber(5);
			{
				Node<ExpressionStatementSymbol>();
				{
					Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.Is(b));
				}
			}
			Node<LineSymbol>()
				.HasLineNumber(6);
			{
				Node<ExpressionStatementSymbol>();
				{
					Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.Is(c));
				}
			}
			Node<LineSymbol>()
				.HasLineNumber(7);
			{
				Node<ExpressionStatementSymbol>();
				{
					Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.Is(d));
				}
			}
		}
		End();

		Assert.Empty(tree.Diagnostics);
	}

}
