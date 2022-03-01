using System;
using System.IO;
using Reline.Common;
using Reline.Compilation.Parsing;
using Reline.Compilation.Binding;

if (args.Length == 0) return;
string path = args[0];
if (!File.Exists(path)) return;
string text = File.ReadAllText(path);

var parseResult = Parser.ParseString(text);
var bindResult = Binder.BindTree(parseResult);

var parseDescendants = parseResult.Root.GetDescendants();
var bindDescendants = bindResult.Root.GetDescendants();

Console.ReadLine();
