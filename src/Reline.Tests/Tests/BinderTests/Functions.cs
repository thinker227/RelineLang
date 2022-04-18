using Reline.Compilation;
using Reline.Compilation.Binding;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Symbols;
using static Reline.Tests.BinderTestUtilities;

namespace Reline.Tests.BinderTests;

public class Functions {

	[Fact]
	public void NativeInvocations() {
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
				.LineNumberIs(1);
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
				.LineNumberIs(2);
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
				.LineNumberIs(3);
			r.SkipTo<LineSymbol>()
				.LineNumberIs(4);
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
				.LineNumberIs(5);
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
				.LineNumberIs(6);
			r.SkipTo<LineSymbol>()
				.LineNumberIs(7);
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
				.LineNumberIs(8);
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
				.LineNumberIs(9);
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
				.LineNumberIs(10);
			r.SkipTo<LineSymbol>()
				.LineNumberIs(11);
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
				.LineNumberIs(12);
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
	public void SimpleDeclarations() {
		string source =
@"function Foo 1..1
function Bar 2..2";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.ParametersAre(0)
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
				.LineNumberIs(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.ParametersAre(0)
						.RangeIs(r => r
							.StartIs(2)
							.EndIs(2))
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
	public void DeclarationsLineExpressions() {
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
				.LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
				.LineNumberIs(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(r => r
							.StartIs(2)
							.EndIs(2))
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
				.LineNumberIs(3);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Baz")
						.RangeIs(r => r
							.StartIs(3)
							.EndIs(3))
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
	public void Pointers() {
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
				.LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
				.LineNumberIs(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(r => r
							.StartIs(2)
							.EndIs(2))
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
				.LineNumberIs(3);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<FunctionPointerExpressionSymbol>()
						.FunctionIs(f => f
							.IsEqualTo(Foo));
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(4);
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
	public void DeclarationsPointers() {
		string source =
@"function Foo 1..1
function Bar *Foo";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
				.LineNumberIs(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
	public void UndeclaredFunction() {
		string source =
@"Foo ()";
		var (r, tree) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BadExpressionSymbol>();
					tree.HasDiagnostic(new TextSpan(0, 3), CompilerDiagnostics.undeclaredFunction);
				}
			}
		}
		r.End();
	}

	[Fact]
	public void EmptyParameters() {
		string source =
@"function Foo 1..1
function Bar 2..2 ()";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
				.LineNumberIs(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(r => r
							.StartIs(2)
							.EndIs(2))
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
	public void Parameters() {
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
			   .LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(func => func
						.IdentifierIs("Foo")
						.RangeIs(r => r
							.StartIs(1)
							.EndIs(1))
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
			   .LineNumberIs(2);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(func => func
						.IdentifierIs("Bar")
						.RangeIs(r => r
							.StartIs(2)
							.EndIs(2))
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
			   .LineNumberIs(3);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(func => func
						.IdentifierIs("Baz")
						.RangeIs(r => r
							.StartIs(3)
							.EndIs(3))
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
	[Fact]
	public void ParameterScope() {
		string source =
@"function Foo 2..3 (a)
a
b

function Bar 6..7 (b)
a
b

a
b";
		var (r, tree) = Compile(source);

		FunctionSymbol Foo = null!;
		FunctionSymbol Bar = null!;
		ParameterSymbol a = null!;
		ParameterSymbol b = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.LineNumberIs(1);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Foo")
						.RangeIs(r => r
							.StartIs(2)
							.EndIs(3))
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
							.HasValue(2);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(3);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(2);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(identifier => identifier
							.IsType<ParameterSymbol>()
							.IsEqualTo(a));
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(3);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BadExpressionSymbol>();
					tree.HasDiagnostic(r.Current, CompilerDiagnostics.undeclaredIdentifier);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(4);
			r.Node<LineSymbol>()
				.LineNumberIs(5);
			{
				r.Node<FunctionDeclarationStatementSymbol>()
					.FunctionIs(f => f
						.IdentifierIs("Bar")
						.RangeIs(r => r
							.StartIs(6)
							.EndIs(7))
						.ArityIs(1)
						.Do(x => Bar = x)
						.ParametersAre(parameters => parameters
							.ElementIs(param => param
								.IdentifierIs("b")
								.FunctionIs(pf => pf
									.IsEqualTo(Bar))
								.Do(x => b = x))
							.End()));
				{
					r.Node<BinaryExpressionSymbol>()
						.OperatorTypeIs(BinaryOperatorType.Range);
					{
						r.Node<LiteralExpressionSymbol>()
							.HasValue(6);
						r.Node<LiteralExpressionSymbol>()
							.HasValue(7);
					}
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(6);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BadExpressionSymbol>();
					tree.HasDiagnostic(r.Current, CompilerDiagnostics.undeclaredIdentifier);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(7);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(identifier => identifier
							.IsType<ParameterSymbol>()
							.IsEqualTo(b));
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(8);
			r.Node<LineSymbol>()
				.LineNumberIs(9);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BadExpressionSymbol>();
					tree.HasDiagnostic(r.Current, CompilerDiagnostics.undeclaredIdentifier);
				}
			}
			r.Node<LineSymbol>()
				.LineNumberIs(10);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<BadExpressionSymbol>();
					tree.HasDiagnostic(r.Current, CompilerDiagnostics.undeclaredIdentifier);
				}
			}
		}
		r.End();
	}

}
