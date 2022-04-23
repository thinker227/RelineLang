using System.Linq;
using Reline.Compilation.Syntax;

namespace Reline.Tests;

public class SyntaxTokenEqualityComparer : IEqualityComparer<SyntaxToken> {

	public SyntaxTokenComparison Comparison { get; }



	public SyntaxTokenEqualityComparer(SyntaxTokenComparison comparison) {
		Comparison = comparison;
	}



	public bool Equals(SyntaxToken x, SyntaxToken y) =>
		(Comparison.HasFlag(SyntaxTokenComparison.IgnoreType) ||
		x.Type == y.Type) &&

		(Comparison.HasFlag(SyntaxTokenComparison.IgnoreSpan) ||
		x.Span == y.Span) &&

		(Comparison.HasFlag(SyntaxTokenComparison.IgnoreText) ||
		x.Text == y.Text) &&

		(Comparison.HasFlag(SyntaxTokenComparison.IgnoreLiteral) ||
		(x.Literal?.Equals(y.Literal) ?? y.Literal is null)) &&

		(Comparison.HasFlag(SyntaxTokenComparison.IgnoreTrivia) ||
		(x.LeadingTrivia.SequenceEqual(y.LeadingTrivia) &&
		 x.TrailingTrivia.SequenceEqual(y.TrailingTrivia)));
	
	public int GetHashCode(SyntaxToken obj) =>
		throw new NotSupportedException();

}

[Flags]
public enum SyntaxTokenComparison {
	IgnoreType = 1,
	IgnoreSpan = 2,
	IgnoreText = 4,
	IgnoreLiteral = 8,
	IgnoreTrivia = 16,

	OnlyToken = IgnoreSpan | IgnoreTrivia
}
