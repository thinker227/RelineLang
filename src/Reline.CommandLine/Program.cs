using System;
using System.IO;
using System.Linq;
using Humanizer;
using Reline.Common.Text;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Syntax;
using Reline.Compilation.Symbols;
using Reline.Compilation.Transpilation.Javascript;

const string version = "v1.0.0-dev";
Console.WriteLine($"--- Reline {version} ---");

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

#if RELEASE
var func = () => {
	var parseResult = SyntaxTree.ParseString(text);
	var bindResult = SemanticModel.BindTree(parseResult);
	return (parseResult, bindResult);
};
var task = Task.Run(func);
const int timeout = 2000;
bool finished = Task.WaitAll(new[] { task }, timeout);
if (!finished) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine($"Compilation timed out ({timeout}ms)");
	Console.ResetColor();
	return 1;
}
var (parseResult, bindResult) = task.Result;
#else
var parseResult = SyntaxTree.ParseString(text);
var bindResult = SemanticModel.BindTree(parseResult);
#endif

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

if (errors > 0) return 1;

JavascriptTranspilationOptions options = new() {
	Target = JavascriptTarget.Node
};
var transpiled = (NodeResult)JavascriptTranspiler.Transpile(bindResult, options);
string transpiledPath = Path.ChangeExtension(file.FullName, ".js");
File.WriteAllText(transpiledPath, transpiled.Source.Source);
Console.WriteLine($"Generated transpied source file '{transpiledPath}'");

return 0;
