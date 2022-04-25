using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using Humanizer;
using Reline.Common.Text;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;
using Reline.Compilation.Symbols;

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
		Console.WriteLine();

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
		var diagnostics = tree.Diagnostics.AddRange(model.Diagnostics);
		int diagnosticsCount = diagnostics.Length;
		int warnings = diagnostics
			.Where(d => d.Level == DiagnosticLevel.Warning)
			.Count();
		int errors = diagnostics
			.Where(d => d.Level == DiagnosticLevel.Error)
			.Count();
		bool success = errors == 0;

		var statusMarkup = GetCompilationStatusMarkup(success, warnings, errors);
		var panel = GetCompilationStatusPanel(success, statusMarkup);
		Console.Write(panel);
		Console.WriteLine();

		if (diagnosticsCount > 0) {
			var table = GetDiagnosticsTable(diagnostics);
			Console.Write(table);
			Console.WriteLine();
		}

		return success ? 1 : 0;
	}
	private static Panel GetCompilationStatusPanel(bool success, Markup text) =>
		new Panel(text)
			.BorderColor(success ? Color.Green : Color.Red);
	private static Markup GetCompilationStatusMarkup(bool success, int warnings, int errors) {
		string status = errors == 0 ? "[green]succeeded[/]" : "[red]failed[/]";
		
		string warningsPlural = warnings == 1 ? "warning" : "warnings";
		string errorsPlural = errors == 1 ? "error" : "errors";
		string warningsMarkup = warnings == 0
			? "no warnings"
			: $"[yellow]{warnings} {warningsPlural}[/]";
		string errorsMarkup = errors == 0
			? "no errors"
			: $"[red]{errors} {errorsPlural}[/]";

		string markup = $"Compilation {status} with {warningsMarkup} and {errorsMarkup}";
		return new Markup(markup);
	}
	private Table GetDiagnosticsTable(IEnumerable<Diagnostic> diagnostics) {
		var table = new Table()
			.Title(new TableTitle("Diagnostics", new Style(Color.White)))
			.BorderColor(Color.White)
			.AddColumn("Severity")
			.AddColumn("ID")
			.AddColumn("Description")
			.AddColumn(new TableColumn("Location").Centered());

		var map = TextMap.Create(Source);
		foreach (var diagnostic in diagnostics) {
			string color = GetDiagnosticColorMarkup(diagnostic);

			string severity = $"[{color}]{diagnostic.Level.Humanize(LetterCasing.Sentence)}[/]";
			string id = $"[white]{diagnostic.ErrorCode}[/]";
			string description = $"[white]{diagnostic.Description}[/]";
			var location = GetDiagnosticLocationMarkup(diagnostic, map);

			table.AddRow(
				new Markup(severity),
				new Markup(id),
				new Markup(description),
				location);
		}

		return table;
	}
	private static string GetDiagnosticColorMarkup(Diagnostic diagnostic) =>
		(diagnostic.Level switch {
			DiagnosticLevel.Hidden => Color.Grey,
			DiagnosticLevel.Info => Color.NavajoWhite1,
			DiagnosticLevel.Warning => Color.Yellow,
			DiagnosticLevel.Error => Color.Red,
			_ => Color.Grey
		}).ToMarkup();
	private Markup GetDiagnosticLocationMarkup(Diagnostic diagnostic, TextMap map) {
		string color = GetDiagnosticColorMarkup(diagnostic);

		if (diagnostic.Location is null)
			return new Markup($"[grey]<[/][{color}]global[/][grey]>[/]");

		StringBuilder builder = new();
		var source = Source.AsSpan();
		var lines = map.GetLineAt(diagnostic.Location.Value);
		foreach (var (lineNumber, span, _) in lines) {
			var diagnosticSpan = diagnostic.Location.Value;
			var init = source.Slice(new TextSpan(span.Start, diagnosticSpan.Start));
			var body = source.Slice(diagnosticSpan);
			var end = source.Slice(new TextSpan(diagnosticSpan.End, span.End)).TrimEnd();

			string markup = $"[grey]{lineNumber}: [/][white]{init}[/][{color}]{body}[/][white]{end}[/]";
			builder.AppendLine(markup);
		}

		return new Markup(builder.ToString());
	}

	private int HandleTimeout() {
		string markup = $"Compilation [red]timed out[/] [grey]({Args.Timeout} ms)[/]";
		var panel = GetCompilationStatusPanel(false, new Markup(markup));
		Console.Write(panel);
		Console.WriteLine();
		return 1;
	}
	private int HandleInternalException(Exception exception) {
		if (exception is AggregateException aggregate)
			exception = aggregate.InnerException!;

		var renderableException = exception.GetRenderable();
		var table = new Table()
			.BorderColor(Color.Red)
			.AddColumn(new TableColumn(new Markup("Compilation [red]failed due to an internal exception[/]")))
			.AddRow(renderableException);
		Console.Write(table);
		Console.WriteLine();
		return 1;
	}

}
