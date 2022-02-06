using System;
using System.IO;
using Reline.Compilation;
using Reline.Compilation.Lexing;
using Reline.Compilation.Parsing;
using Reline.Compilation.Binding;

if (args.Length == 0) return;
var path = args[0];
if (!File.Exists(path)) return;
var text = File.ReadAllText(path);

var parseResult = Parser.ParseString(text);

var bindResult = Binder.BindTree(parseResult);

Console.ReadLine();
