using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Reline.CommandLine;

#nullable disable
internal sealed class CompilationArgs : CommandSettings {

	[CommandArgument(0, "<file>")]
	public string FilePath { get; set; }
	public FileInfo File => new(FilePath);

	[CommandOption("--timeout <timeout>")]
	public int Timeout { get; set; } = 2000;



	public override ValidationResult Validate() {
		if (!File.Exists) {
			return ValidationResult.Error($"File '{File.FullName}' does not exist.");
		}
		
		return ValidationResult.Success();
	}

}
