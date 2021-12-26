using System;
using System.IO;
using Reline.Compilation.Lexing;

if (args.Length == 0) return;
var path = args[0];
if (!File.Exists(path)) return;
var text = File.ReadAllText(path);

Lexer lexer = new(text);
var tokens = lexer.LexAll();

Console.ReadLine();
