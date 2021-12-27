using System;
using System.IO;
using Reline.Compilation.Lexing;
using Reline.Compilation.Parsing;

if (args.Length == 0) return;
var path = args[0];
if (!File.Exists(path)) return;
var text = File.ReadAllText(path);

var lexResult = Lexer.LexSource(text);

Parser parser = new(lexResult);
var parseResult = parser.ParseAll();

Console.ReadLine();
