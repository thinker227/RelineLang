using Reline.Compilation;
using Reline.Compilation.Binding;
using Reline.Compilation.Symbols;

namespace Reline.Tests.BinderTests;

public class Functions : BinderTestBase {

	[Fact]
	public void NativeFunctionInvocations() {
		string source =
@"Write (""Hello world!"")
ReadLine ()

String (27)
ParseInt (""76"")

Clamp (1 2 3)
Min (1 2)
Max (3 4)

StringIndex (""abc"" 2)
Ascii (""a"")";
		var (r, tree) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("Write")
							.FunctionTypeIs(NativeFunction.Write));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(2);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("ReadLine")
							.FunctionTypeIs(NativeFunction.ReadLine));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(3);
			r.SkipTo<LineSymbol>()
				.HasLineNumber(4);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("String")
							.FunctionTypeIs(NativeFunction.String));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(5);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("ParseInt")
							.FunctionTypeIs(NativeFunction.ParseInt));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(6);
			r.SkipTo<LineSymbol>()
				.HasLineNumber(7);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("Clamp")
							.FunctionTypeIs(NativeFunction.Clamp));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(8);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("Min")
							.FunctionTypeIs(NativeFunction.Min));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(9);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("Max")
							.FunctionTypeIs(NativeFunction.Max));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(10);
			r.SkipTo<LineSymbol>()
				.HasLineNumber(11);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("StringIndex")
							.FunctionTypeIs(NativeFunction.StringIndex));
				}
			}
			r.SkipTo<LineSymbol>()
				.HasLineNumber(12);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionInvocationExpressionSymbol>()
						.FunctionIs(f => f
							.IsType<NativeFunctionSymbol>()
							.IdentifierIs("Ascii")
							.FunctionTypeIs(NativeFunction.Ascii));
				}
			}
		}

		Assert.Empty(tree.Diagnostics);
	}
	[Fact]
	public void SimpleFunctionDeclarations() {
		string source =
@"function Foo 1..1
function Bar 2..2";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.ParametersAre(0)
						.RangeIs(new RangeValue(1, 1))
						.Do(x => Foo = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.ParametersAre(0)
						.RangeIs(new RangeValue(2, 2))
						.Do(x => Bar = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
					}
				}
			}
		}
		r.End();

		Assert.Contains(Foo, tree.Functions);
		Assert.Contains(Bar, tree.Functions);
	}
	[Fact]
	public void FunctionDeclarationsLineExpressions() {
		string source =
@"function Foo start..start
function Bar here..here
function Baz end..end";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;
		FunctionSymbol Baz = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(new RangeValue(1, 1))
						.Do(x => Foo = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<KeywordExpressionSymbol>()
							.KeywordIs(KeywordExpressionType.Start);
						r.Node<KeywordExpressionSymbol>()
							.KeywordIs(KeywordExpressionType.Start);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(new RangeValue(2, 2))
						.Do(x => Bar = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<KeywordExpressionSymbol>()
							.KeywordIs(KeywordExpressionType.Here);
						r.Node<KeywordExpressionSymbol>()
							.KeywordIs(KeywordExpressionType.Here);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(3);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Baz")
						.RangeIs(new RangeValue(3, 3))
						.Do(x => Baz = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<KeywordExpressionSymbol>()
							.KeywordIs(KeywordExpressionType.End);
						r.Node<KeywordExpressionSymbol>()
							.KeywordIs(KeywordExpressionType.End);
					}
				}
			}
		}
		r.End();

		Assert.Contains(Foo, tree.Functions);
		Assert.Contains(Bar, tree.Functions);
		Assert.Contains(Baz, tree.Functions);

		Assert.Empty(tree.Diagnostics);
	}
	[Fact]
	public void FunctionPointers() {
		string source =
@"function Foo 1..1
function Bar 2..2
*Foo
*Bar";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(new RangeValue(1, 1))
						.Do(x => Foo = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(new RangeValue(2, 2))
						.Do(x => Bar = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(3);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionPointerExpressionSymbol>()
						.FunctionIs(f => f
							.IsEqualTo(Foo));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(4);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionPointerExpressionSymbol>()
						.FunctionIs(f => f
							.IsEqualTo(Bar));
				}
			}
		}
		r.End();

		Assert.Contains(Foo, tree.Functions);
		Assert.Contains(Bar, tree.Functions);

		Assert.Empty(tree.Diagnostics);
	}
	[Fact]
	public void FunctionDeclarationsFunctionPointers() {
		string source =
@"function Foo 1..1
function Bar *Foo";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(new RangeValue(1, 1))
						.Do(x => Foo = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(new RangeValue(1, 1))
						.Do(x => Bar = x));
				{
					r.Node<FunctionPointerExpressionSymbol>()
						.FunctionIs(f => f
							.IsEqualTo(Foo));
				}
			}
		}
		r.End();

		Assert.Contains(Foo, tree.Functions);
		Assert.Contains(Bar, tree.Functions);

		Assert.Empty(tree.Diagnostics);
	}

	[Fact]
	public void EmptyFunctionParameters() {
		string source =
@"function Foo 1..1
function Bar 2..2 ()";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(new RangeValue(1, 1))
						.ArityIs(0)
						.Do(x => Foo = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(new RangeValue(2, 2))
						.ArityIs(0)
						.Do(x => Bar = x));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
					}
				}
			}
		}
		r.End();

		Assert.Contains(Foo, tree.Functions);
		Assert.Contains(Bar, tree.Functions);

		Assert.Empty(tree.Diagnostics);
	}
	[Fact]
	public void FunctionParameters() {
		string source =
@"function Foo 1..1 (a)
function Bar 2..2 (b c)
function Baz 3..3 (d e f)";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;
		FunctionSymbol Baz = null!;
		ParameterSymbol a = null!;
		ParameterSymbol b = null!;
		ParameterSymbol c = null!;
		ParameterSymbol d = null!;
		ParameterSymbol e = null!;
		ParameterSymbol f = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				   .HasLineNumber(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(func => func
						.IdentifierIs("Foo")
						.RangeIs(new RangeValue(1, 1))
						.ArityIs(1)
						.Do(x => Foo = x)
						.ParametersAre(parameters => parameters
							.ElementIs(param => param
								.IdentifierIs("a")
								.FunctionIs(pf => pf
									.IsEqualTo(Foo))
								.Do(x => a = x))
							.End()));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(1);
					}
				}
			}
			r.Node<LineSymbol>()
				   .HasLineNumber(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(func => func
						.IdentifierIs("Bar")
						.RangeIs(new RangeValue(2, 2))
						.ArityIs(2)
						.Do(x => Bar = x)
						.ParametersAre(parameters => parameters
							.ElementIs(param => param
								.IdentifierIs("b")
								.FunctionIs(pf => pf
									.IsEqualTo(Bar))
								.Do(x => b = x))
							.ElementIs(param => param
								.IdentifierIs("c")
								.FunctionIs(pf => pf
									.IsEqualTo(Bar))
								.Do(x => c = x))
							.End()));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(2);
					}
				}
			}
			r.Node<LineSymbol>()
				   .HasLineNumber(3);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(func => func
						.IdentifierIs("Baz")
						.RangeIs(new RangeValue(3, 3))
						.ArityIs(3)
						.Do(x => Baz = x)
						.ParametersAre(parameters => parameters
							.ElementIs(param => param
								.IdentifierIs("d")
								.FunctionIs(pf => pf
									.IsEqualTo(Baz))
								.Do(x => d = x))
							.ElementIs(param => param
								.IdentifierIs("e")
								.FunctionIs(pf => pf
									.IsEqualTo(Baz))
								.Do(x => e = x))
							.ElementIs(param => param
								.IdentifierIs("f")
								.FunctionIs(pf => pf
									.IsEqualTo(Baz))
								.Do(x => f = x))
							.End()));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
					}
				}
			}
		}
		r.End();

		Assert.Contains(Foo, tree.Functions);
		Assert.Contains(Bar, tree.Functions);
		Assert.Contains(Baz, tree.Functions);
		Assert.Contains(a, tree.Variables);
		Assert.Contains(b, tree.Variables);
		Assert.Contains(c, tree.Variables);
		Assert.Contains(d, tree.Variables);
		Assert.Contains(e, tree.Variables);
		Assert.Contains(f, tree.Variables);

		Assert.Empty(tree.Diagnostics);
	}

}
