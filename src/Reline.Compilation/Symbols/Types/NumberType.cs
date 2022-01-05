namespace Reline.Compilation.Symbols;

public sealed class NumberType : SymbolNode, ITypeSymbol {

	public static NumberType Instance { get; } = new();
	public bool IsNative => true;
	public string TypeName => "Number";



	private NumberType() { }



	public bool Equals(ITypeSymbol? other) =>
		other is NumberType;
	public override bool Equals(object? obj) =>
		obj is NumberType;
	public override int GetHashCode() =>
		HashCode.Combine(TypeName);

}
