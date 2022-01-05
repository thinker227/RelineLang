namespace Reline.Compilation.Symbols;

public sealed class StringType : SymbolNode, ITypeSymbol {

	public static StringType Instance { get; } = new();
	public bool IsNative => true;
	public string TypeName => "String";



	private StringType() { }



	public bool Equals(ITypeSymbol? other) =>
		other is StringType;
	public override bool Equals(object? obj) =>
		obj is StringType;
	public override int GetHashCode() =>
		HashCode.Combine(TypeName);

}
