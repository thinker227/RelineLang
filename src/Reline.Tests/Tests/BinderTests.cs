﻿using Reline.Compilation;
using Reline.Compilation.Binding;
using Reline.Compilation.Symbols;

namespace Reline.Tests;

public class BinderTests : BinderTestBase {

	[Fact]
	public void LineNumbers() {
		const int lines = 200;
		string source = new('\n', lines - 1);
		var (r, tree) = Compile(source);

		r.Node<ProgramSymbol>();
		{
			for (int i = 0; i < lines; i++) {
				r.Node<LineSymbol>()
					.HasLineNumber(i + 1);
			}
		}
		r.End();

		Assert.Empty(tree.Diagnostics);
	}

	[Fact]
	public void SimpleLabelsAndVariables() {
		string source =
@"a: b = 0";
		var (r, tree) = Compile(source);

		LabelSymbol a = null!;
		VariableSymbol b = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1)
				.LabelIs(l => l
					.IdentifierIs("a")
					.Do(x => a = x));
			{
				r.Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IsType<VariableSymbol>()
						.IdentifierIs("b")
						.Do(x => b = x));
				{
					r.Node<LiteralExpressionSymbol>();
				}
			}
		}
		r.End();

		Assert.Contains(a, tree.Labels);
		Assert.Contains(b, tree.Variables);

		Assert.Empty(tree.Diagnostics);
	}
	[Fact]
	public void SimpleLabelAndVariableReferences() {
		string source =
@"a: b = 2
c: d = ""uwu""

a
b
c
d";
		var (r, tree) = Compile(source);

		LabelSymbol a = null!;
		IVariableSymbol b = null!;
		LabelSymbol c = null!;
		IVariableSymbol d = null!;

		r.Node<ProgramSymbol>();
		{
			r.Node<LineSymbol>()
				.HasLineNumber(1)
				.LabelIs(l => l
					.IdentifierIs("a")
					.Do(x => a = x));
			{
				r.Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("b")
						.Do(x => b = x));
				{
					r.Node<LiteralExpressionSymbol>()
						.HasValue(2);
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(2)
				.LabelIs(l => l
					.IdentifierIs("c")
					.Do(x => c = x));
			{
				r.Node<AssignmentStatementSymbol>()
					.VariableIs(v => v
						.IdentifierIs("d")
						.Do(x => d = x));
				{
					r.Node<LiteralExpressionSymbol>()
						.HasValue("uwu");
				}
			}
			r.Node<LineSymbol>();
			r.Node<LineSymbol>()
				.HasLineNumber(4);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(a));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(5);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(b));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(6);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(c));
				}
			}
			r.Node<LineSymbol>()
				.HasLineNumber(7);
			{
				r.Node<ExpressionStatementSymbol>();
				{
					r.Node<IdentifierExpressionSymbol>()
						.IdentifierIs(i => i
							.IsEqualTo(d));
				}
			}
		}
		r.End();

		Assert.Empty(tree.Diagnostics);
	}

	[Fact]
	public void NativeFunctionInvocation() {
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

}
