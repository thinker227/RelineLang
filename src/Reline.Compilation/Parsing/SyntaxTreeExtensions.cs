using Reline.Compilation.Parsing;
using Reline.Compilation.Syntax.Nodes;

namespace Reline.Compilation;

internal static class SyntaxTreeExtensions {

	public static IEnumerable<TStatement> GetStatementsOfType<TStatement>(this SyntaxTree tree) where TStatement : IStatementSyntax {
		foreach (var line in tree.Root.Lines) {
			var statement = line.Statement;
			if (statement is TStatement s)
				yield return s;
		}
	}

}
