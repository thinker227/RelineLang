using Reline.Compilation.Symbols;
using Shouldly;

namespace Reline.Tests.BinderTests;

public class Basic : BinderTestBase {

	[Fact]
	public void LineNumbers() {
		const int lines = 200;
		string source = new('\n', lines - 1);
		var (r, tree) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			for (int i = 0; i < lines; i++) {
				r.Node<LineSymbol>()
					.LineNumberIs(i + 1);
			}
		}
		r.End();

		Assert.Empty(tree.Diagnostics);
	}

}
