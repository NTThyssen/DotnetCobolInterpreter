// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;
using CobolFieldAnalyzer;

string input = File.ReadAllText("TestFiles\\testfile.cbl"); // Replace with your test input

AntlrInputStream inputStream = new AntlrInputStream(input);

var dataLexer = new CobolLexer(inputStream);
CommonTokenStream tokenStream = new CommonTokenStream(dataLexer);
var dataDivsionParser = new CobolParser(tokenStream);

// Parse the input and get the parse tree (replace 'startRule' with your starting rule)
var tree = dataDivsionParser.startRule();
var symbolTable  =  SymbolTable.Instance;
var visitor = new CustomDataDivsionVisitor(symbolTable);
visitor.Visit(tree);




