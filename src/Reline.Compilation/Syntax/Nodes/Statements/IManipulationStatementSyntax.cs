namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a line manipulation statement.
/// </summary>
public interface IManipulationStatementSyntax : IStatementSyntax {

	/// <summary>
	/// The keyword signifying the source.
	/// </summary>
	SyntaxToken SourceKeyword { get; }
	/// <summary>
	/// The source expression.
	/// </summary>
	IExpressionSyntax Source { get; }
	/// <summary>
	/// The keyword signifying the target.
	/// </summary>
	SyntaxToken TargetKeyword { get; }
	/// <summary>
	/// The target expression.
	/// </summary>
	IExpressionSyntax Target { get; }

}
