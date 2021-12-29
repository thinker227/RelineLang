using System;
using System.IO;
using Reline.Compilation;
using Reline.Compilation.Lexing;
using Reline.Compilation.Parsing;

if (args.Length == 0) return;
var path = args[0];
if (!File.Exists(path)) return;
var text = File.ReadAllText(path);

var lexResult = Lexer.LexSource(text);
if (lexResult.HasErrors()) return;

var parseResult = Parser.ParseTokens(lexResult);

Console.ReadLine();
