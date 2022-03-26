using System;
using System.IO;
using System.Linq;
using Reline.Common.Text;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Parsing;
using Reline.Compilation.Binding;
using Humanizer;


if (args.Length == 0) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine("No path was provided");
	Console.ResetColor();
	return 1;
}
FileInfo file = new(args[0]);
string path = file.FullName;
if (!file.Exists) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine($"File '{path}' does not exist");
	Console.ResetColor();
	return 1;
}
if (file.Extension != ".rl") {
	Console.ForegroundColor = ConsoleColor.Yellow;
	Console.WriteLine("File does not have extension '.rl'");
	Console.ResetColor();
}
string text = File.ReadAllText(path);
Console.WriteLine($"Compiling file '{path}'\n");

var textMap = TextMap.Create(text);

var parseResult = Parser.ParseString(text);
var bindResult = Binder.BindTree(parseResult);

var diagnostics = parseResult.Diagnostics.AddRange(bindResult.Diagnostics);
foreach (var diagnostic in diagnostics) {
	var color = diagnostic.Level switch {
		DiagnosticLevel.Hidden => ConsoleColor.DarkGray,
		DiagnosticLevel.Info => ConsoleColor.Gray,
		DiagnosticLevel.Warning => ConsoleColor.Yellow,
		DiagnosticLevel.Error => ConsoleColor.Red,
		_ => ConsoleColor.White
	};

	Console.ForegroundColor = color;
	Console.Write($"{diagnostic.Level.Humanize(LetterCasing.Sentence)} {diagnostic.ErrorCode}: ");
	Console.ForegroundColor = ConsoleColor.Gray;
	Console.WriteLine(diagnostic.Description);

	if (diagnostic.Location is null) continue;
	var location = diagnostic.Location.Value;
	foreach (var line in textMap.GetLineAt(location)) {
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write($"{line.LineNumber + 1}: ");

		var l = TextSpan.Union(location, line.TextSpan);
		string start = text.Substring(new TextSpan(line.TextSpan.Start, l.Start));
		string mid = text.Substring(l);
		string end = text.Substring(new TextSpan(l.End, line.TextSpan.End));
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(start.ReplaceLineEndings(""));
		Console.ForegroundColor = color;
		Console.Write(mid.ReplaceLineEndings(""));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(end.ReplaceLineEndings(""));
	}
	Console.ResetColor();
	Console.WriteLine();
}

int warnings = diagnostics.Where(d => d.Level == DiagnosticLevel.Warning).Count();
int errors = diagnostics.Where(d => d.Level == DiagnosticLevel.Error).Count();
bool success = errors == 0;
string warningsString = warnings == 1 ? "warning" : "warnings";
string errorsString = errors == 1 ? "error" : "errors";

if (success) {
	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine("Compilation succeeded");
	Console.ResetColor();
} else {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine("Compilation failed");
	Console.ResetColor();
}
Console.WriteLine($"  {warnings} {warningsString}");
Console.WriteLine($"  {errors} {errorsString}");
Console.WriteLine();

return errors == 0 ? 0 : 1;
