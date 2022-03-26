using System;
using System.IO;
using System.Linq;
using Reline.Common.Text;
using Reline.Compilation.Diagnostics;
using Reline.Compilation.Parsing;
using Reline.Compilation.Binding;
using Humanizer;

if (args.Length == 0) return;
string path = args[0];
if (!File.Exists(path)) return;
string text = File.ReadAllText(path);

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
