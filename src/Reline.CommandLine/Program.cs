using System;
using System.IO;
using Reline.Compilation.Syntax;
using Reline.Compilation.Symbols;

if (args.Length == 0) return;
string path = args[0];
if (!File.Exists(path)) return;
string text = File.ReadAllText(path);

var parseResult = SyntaxTree.ParseString(text);
var bindResult = SemanticModel.BindTree(parseResult);

Console.ReadLine();
