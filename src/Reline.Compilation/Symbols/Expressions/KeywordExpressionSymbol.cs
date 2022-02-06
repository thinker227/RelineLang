﻿using Reline.Compilation.Binding;

namespace Reline.Compilation.Symbols;

public sealed class KeywordExpressionSymbol : SymbolNode, IExpressionSymbol {

	public KeywordExpressionType KeywordType { get; set; }
	public bool IsConstant => false;

	public T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitKeyword(this);

}
