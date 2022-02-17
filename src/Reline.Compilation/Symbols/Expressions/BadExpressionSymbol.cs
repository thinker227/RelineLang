﻿namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents an expression which is not semantically valid.
/// </summary>
public sealed class BadExpressionSymbol : SymbolNode, IExpressionSymbol {

	public T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotSupportedException();

}