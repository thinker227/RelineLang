using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

internal sealed class FunctionBinder : IdentifierBinder<IFunctionSymbol> {

	private static readonly Dictionary<NativeFunction, NativeFunctionSymbol> nativeFunctionCache = new();
	private static NativeFunctionSymbol? GetNativeFunction(string identifier) {
		(NativeFunction, int)? function = identifier switch {
			"Write" => (NativeFunction.Write, 1),
			"ReadLine" => (NativeFunction.ReadLine, 0),

			"String" => (NativeFunction.String, 1),
			"ParseInt" => (NativeFunction.ParseInt, 1),

			"Clamp" => (NativeFunction.Clamp, 3),
			"Min" => (NativeFunction.Min, 2),
			"Max" => (NativeFunction.Max, 2),

			"StringIndex" => (NativeFunction.StringIndex, 2),
			"Ascii" => (NativeFunction.Ascii, 1),

			_ => null
		};

		if (function is null) return null;

		var (type, arity) = function.Value;
		if (nativeFunctionCache.TryGetValue(type, out var cached)) return cached;
		NativeFunctionSymbol symbol = new(identifier, arity, type);
		nativeFunctionCache.Add(type, symbol);
		return symbol;
	}

	public override IFunctionSymbol? GetSymbol(string identifier) =>
		GetNativeFunction(identifier) ??
		base.GetSymbol(identifier);
	public override bool IsDefined(string identifier) =>
		GetNativeFunction(identifier) is not null ||
		base.IsDefined(identifier);

	public IEnumerable<FunctionSymbol> GetDefinedFunctions() =>
		symbols.Values.OfType<FunctionSymbol>();

}
