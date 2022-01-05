namespace Reline.Compilation.Symbols;

public sealed class LineRangeType : SymbolNode, ITypeSymbol {

	public static LineRangeType Instance => new();
	public bool IsNative => true;
	public string TypeName => "LineRange";



	private LineRangeType() { }



	public bool Equals(ITypeSymbol? other) =>
		other is LineRangeType;
	public override bool Equals(object? obj) =>
		obj is LineRangeType;
	public override int GetHashCode() =>
		HashCode.Combine(TypeName);

}
