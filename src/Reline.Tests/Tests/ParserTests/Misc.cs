using Reline.Compilation.Syntax;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Tests.ParserTests;

public class Misc : ParserTestBase {

	[Fact]
	public void ParsesProgramStructure() {
		string source =
@"

";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			r.Node<LineSyntax>();
			r.Node<LineSyntax>();
			r.Node<LineSyntax>();
		}
		r.End();
	}

	[Fact]
	public void ParsesFunctionDeclarations() {
		string source =
@"function Foo 2..3 (a b c)

";
		var (r, _) = Parse(source);

		r.Node<ProgramSyntax>();
		{
			// function Foo 2..3 (a b c)
			r.Node<LineSyntax>();
			{
				// function
				r.Node<FunctionDeclarationStatementSyntax>()
					.IdentifierIs("Foo");
				// 2..3
				{
					// ..
					r.Node<BinaryExpressionSyntax>()
						.OperatorTypeIs(SyntaxType.DotDotToken);
					// 2
					r.Node<LiteralExpressionSyntax>()
						.ValueEquals(2);
					// 3
					r.Node<LiteralExpressionSyntax>()
						.ValueEquals(3);
				}
				// (a b c)
				r.Node<ParameterListSyntax>()
					.ParametersAre(new[] { "a", "b", "c" });
			}
			r.Node<LineSyntax>();
			r.Node<LineSyntax>();
		}
		r.End();
	}

}
