namespace Reline.Compilation.Symbols;

public sealed class RangeType : SymbolNode, ITypeSymbol {

	public static RangeType Instance => new();
	public bool IsNative => true;
	public string TypeName => "range";



	private RangeType() { }



	public bool Equals(ITypeSymbol? other) =>
		other is RangeType;
	public override bool Equals(object? obj) =>
		obj is RangeType;
	public override int GetHashCode() =>
		HashCode.Combine(TypeName);

}
