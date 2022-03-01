﻿namespace Reline.Compilation.Syntax.Nodes;

/// <summary>
/// Represents a syntax node.
/// </summary>
public interface ISyntaxNode : INode<ISyntaxNode> {

	T Accept<T>(ISyntaxVisitor<T> visitor);
	TextSpan GetTextSpan();

}
