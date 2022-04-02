using Reline.Compilation.Symbols;

namespace Reline.Tests;

public class BinderTests : BinderTestBase {

	[Fact]
	public void ExpressionStatementFunctionInvocation() {
		string source =
@"Write (""Hello world!"")";
		var tree = SetTree(source);

		Node<ProgramSymbol>();
		{
			Node<LineSymbol>();
			{
				Node<ExpressionStatementSymbol>();
				{
					Node<FunctionInvocationExpressionSymbol>();
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
			var line = Node<LineSymbol>();
			Assert.NotNull(line.Label);
			Assert.Equal("a", line.Label!.Identifier);
			{
				var assignment = Node<AssignmentStatementSymbol>();
				Assert.NotNull(assignment.Variable);
				Assert.Equal("b", assignment.Variable!.Identifier);
				{
					Node<LiteralExpressionSymbol>();
				}
			}
		}
		End();

		Assert.Empty(tree.Diagnostics);
	}

}
