using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Reline.CommandLine;

internal sealed class CompilationProcess : Command<CompilationArgs> {

// These properties are always set by Execute
#nullable disable
	private IAnsiConsole Console { get; set; }
	private IAnsiConsole Error { get; set; }
#nullable restore

	public override int Execute([NotNull] CommandContext context, [NotNull] CompilationArgs settings) {
		Console = AnsiConsole.Console;
		Error = AnsiConsole.Create(new AnsiConsoleSettings {
			Out = new AnsiConsoleOutput(System.Console.Error)
		});

		var path = GetTextPath(settings.File.FullName);
		if (!settings.File.Exists) {
			Error.WriteLine("File does not exist:", Color.Red);
			Error.Write(path);
			return 1;
		}

		return 0;
	}

	private static TextPath GetTextPath(string fileName) => new TextPath(fileName)
		.RootColor(Color.Lime)
		.SeparatorColor(Color.Red)
		.StemColor(Color.Orange1)
		.LeafColor(Color.Aqua);

}
