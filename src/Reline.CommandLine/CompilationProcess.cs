using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Reline.Compilation.Symbols;
using Reline.Compilation.Syntax;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Reline.CommandLine;

internal sealed class CompilationProcess : Command<CompilationArgs> {

// These properties are always set by Execute
#nullable disable
	private IAnsiConsole Console { get; set; }
	private IAnsiConsole Error { get; set; }
	private CompilationArgs Args { get; set; }
	private string Source { get; set; }
#nullable restore

	public override int Execute([NotNull] CommandContext context, [NotNull] CompilationArgs args) {
		Args = args;
		Console = AnsiConsole.Console;
		Error = AnsiConsole.Create(new AnsiConsoleSettings {
			Out = new AnsiConsoleOutput(System.Console.Error)
		});

		// Args.File has already been validated in CompilationArgs.Validate()
		if (Args.File.Extension != ".rl") {
			Console.WriteMarkup("[yellow]File does not contain extension '[aqua].rl[/]'[/]\n\n");
		}

		Source = File.ReadAllText(Args.File.FullName);

		var path = GetTextPath(Args.File.FullName);
		Console.Write("Compiling file ");
		Console.Write(path);

		var compilationStatus = Console.Status();
		compilationStatus.Spinner = Spinner.Known.SimpleDotsScrolling;
		var result = compilationStatus.Start("Compiling", _ => GetCompilationResult());

		return result switch {
			CompilationResult.Success s => HandleSuccess(s.SyntaxTree, s.SemanticModel),
			CompilationResult.Timeout => HandleTimeout(),
			CompilationResult.InternalException e => HandleInternalException(e.Exception),
			_ => 1
		};
	}

	private static TextPath GetTextPath(string fileName) => new TextPath(fileName)
		.RootColor(Color.Lime)
		.SeparatorColor(Color.Red)
		.StemColor(Color.Orange1)
		.LeafColor(Color.Aqua);

	private CompilationResult GetCompilationResult() {
		// Passing cancellation tokens around would
		// require also supporting that within the compiler.
		// This is a lot easier.
		try {
			var task = Task.Run(Compile);
			int timeout = Args.Timeout <= 0 || Debugger.IsAttached ?
				Timeout.Infinite : Args.Timeout;
			bool success = Task.WaitAll(new[] { task }, timeout);

			if (!success) return new CompilationResult.Timeout();

			var (tree, model) = task.Result;
			return new CompilationResult.Success(tree, model);
		} catch (Exception e) {
			return new CompilationResult.InternalException(e);
		}
	}
	private (SyntaxTree, SemanticModel) Compile() {
		var tree = SyntaxTree.ParseString(Source);
		var model = SemanticModel.BindTree(tree);
		return (tree, model);
	}

	private int HandleSuccess(SyntaxTree tree, SemanticModel model) {
		Console.WriteLine("Compilation successful", Color.Lime);

		var diagnostics = tree.Diagnostics.AddRange(model.Diagnostics);

		return 0;
	}
	private int HandleTimeout() {
		Console.WriteLine($"Compilation timed out ({Args.Timeout} ms)", Color.Red);
		return 1;
	}
	private int HandleInternalException(Exception exception) {
		if (exception is AggregateException aggregate)
			exception = aggregate.InnerException!;

		Console.WriteLine("Compilation failed due to an internal compiler exception:");
		Console.WriteException(exception);
		return 1;
	}

}
