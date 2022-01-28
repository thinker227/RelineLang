﻿namespace Reline.Compilation.Symbols;

/// <summary>
/// Represents a range literal.
/// </summary>
public readonly struct RangeLiteral : IEquatable<RangeLiteral> {

	/// <summary>
	/// The start location of the range, inclusive.
	/// </summary>
	public int Start { get; }
	/// <summary>
	/// The end location of the range, inclusive.
	/// </summary>
	public int End { get; }
	/// <summary>
	/// The length of the range.
	/// </summary>
	public int Length => End - Start + 1;



	/// <summary>
	/// Initializes a new <see cref="RangeLiteral"/> instance.
	/// </summary>
	/// <param name="start">The start location of the range, inclusive.</param>
	/// <param name="end">The end location of the range, inclusive.</param>
	public RangeLiteral(int start, int end) {
		Start = start;
		End = end;
	}



	public bool Equals(RangeLiteral other) =>
		Start == other.Start && End == other.End;
	public override bool Equals(object? obj) =>
		obj is RangeLiteral other && Equals(other);
	public override int GetHashCode() =>
		HashCode.Combine(Start, End);
	public override string ToString() =>
		Length == 1 ? Start.ToString() : $"{Start}..{End}";



	public static bool operator ==(RangeLiteral a, RangeLiteral b) =>
		a.Equals(b);
	public static bool operator !=(RangeLiteral a, RangeLiteral b) =>
		!a.Equals(b);

}
