// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;
using DotnetCobolParser;

string input = File.ReadAllText("TestFiles\\testfile.cbl"); // Replace with your test input
AntlrInputStream inputStream = new AntlrInputStream(input);

// Replace 'SimpleGrammarLexer' and 'SimpleGrammarParser' with your generated classes
var lexer = new CobolLexer(inputStream);
CommonTokenStream tokenStream = new CommonTokenStream(lexer);
var parser = new CobolParser(tokenStream);

// Parse the input and get the parse tree (replace 'startRule' with your starting rule)
var tree = parser.startRule();

// Create and attach the listener
var listener = new CustomDataDivisionListener(parser);
Antlr4.Runtime.Tree.ParseTreeWalker.Default.Walk(listener, tree);
