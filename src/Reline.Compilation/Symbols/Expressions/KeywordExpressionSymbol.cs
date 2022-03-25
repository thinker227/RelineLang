using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a keyword expression-
/// </summary>
public sealed class KeywordExpressionSymbol : SymbolNode, IExpressionSymbol {

	/// <summary>
	/// The keyword of the expression.
	/// </summary>
	public KeywordExpressionType KeywordType { get; }



	internal KeywordExpressionSymbol(KeywordExpressionType keywordType) {
		KeywordType = keywordType;
	}



	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitKeyword(this);

	public override IEnumerable<ISymbol> GetChildren() =>
		Enumerable.Empty<ISymbol>();

}
