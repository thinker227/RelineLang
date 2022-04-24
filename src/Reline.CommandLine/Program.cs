using Spectre.Console.Cli;
using Reline.CommandLine;

const string version = "1.0.0-dev";

CommandApp<CompilationProcess> app = new();
app.Configure(config => {
	config.SetApplicationName("Reline");
	config.SetApplicationVersion(version);
});

return app.Run(args);
