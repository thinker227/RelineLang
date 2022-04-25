using Spectre.Console;

namespace Reline.CommandLine;

internal static class Extensions {

	public static void Write(this IAnsiConsole console, string text, Color color) =>
		console.Write(text, new Style(color));
	public static void WriteLine(this IAnsiConsole console, string text, Color color) =>
		console.WriteLine(text, new Style(color));

	public static void WriteMarkup(this IAnsiConsole console, string text, Style? style = null) {
		Markup markup = new(text, style);
		console.Write(markup);
	}

}
