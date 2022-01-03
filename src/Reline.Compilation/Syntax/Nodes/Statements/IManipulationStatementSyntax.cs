namespace Reline.Compilation.Syntax.Nodes;

public interface IManipulationStatementSyntax : IStatementSyntax {

	SyntaxToken SourceKeyword { get; }
	IExpressionSyntax Source { get; }
	SyntaxToken TargetKeyword { get; }
	IExpressionSyntax Target { get; }

}
