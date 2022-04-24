using System.IO;
using Spectre.Console.Cli;

namespace Reline.CommandLine;

#nullable disable
internal sealed class CompilationArgs : CommandSettings {

	[CommandArgument(0, "<file>")]
	public string FilePath { get; set; }
	public FileInfo File => new(FilePath);

	[CommandOption("--do-timeout <do-timeout>")]
	public bool DoTimeout { get; set; } = true;
	[CommandOption("--timeout <timeout>")]
	public int Timeout { get; set; } = 2000;

}
