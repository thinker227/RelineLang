namespace Reline.Compilation.Symbols;

public abstract class BinaryExpressionSymbol : SymbolNode, IExpressionSymbol {

	public IExpressionSymbol Left { get; set; } = null!;
	public IExpressionSymbol Right { get; set; } = null!;
	public virtual ITypeSymbol Type =>
		Left.Type.Equals(Right.Type) ? Left.Type :
		throw new InconclusiveTypeException($"Type of {nameof(Left)} and {nameof(Right)} do not match");
	public virtual bool IsConstant => Left.IsConstant && Right.IsConstant;

}

public abstract class BinaryExpressionSymbol<TType> : BinaryExpressionSymbol, IExpressionSymbol where TType : ITypeSymbol {

	public override ITypeSymbol Type =>
		Left is TType && Right is TType ? Left.Type :
		throw new InconclusiveTypeException($"Type of {nameof(Left)} and {nameof(Right)} do not match type {typeof(TType).Name}");

}
