using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests;

public class ParserTests : ParserTestBase {

	[Fact]
	public void ParsesSimple() {
		string source =
@"Write (""Hello world!"")";
		SetTree(source);

		Node<LineSyntax>();
		{
			Node<ExpressionStatementSyntax>();
			{
				Node<FunctionInvocationExpressionSyntax>();
				{
					var literal = Node<LiteralExpressionSyntax>();
					Assert.Equal("Hello world!", literal.Literal.Literal);
				}
			}
		}
	}

}
