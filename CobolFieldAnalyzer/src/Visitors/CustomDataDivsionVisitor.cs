using Antlr4.Runtime.Tree;
using CobolFieldAnalyzer;

namespace CobolFieldAnalyzer;

public class CustomDataDivsionVisitor : CobolParserBaseVisitor<object> // Replace 'Cobol' with your grammar name
{

    Stack<CobolDataVariable> currentCobolnodeStack = new Stack<CobolDataVariable>();

    SymbolTable symbolTable = SymbolTable.Instance;
    private readonly CobolParser _parser;

    CobolDataVariable root = new CobolDataVariable() { Level = 0 };
    CobolDataVariable currentParent;
    int currentLevelNumer;
    public CustomDataDivsionVisitor(SymbolTable table)
    {
        currentParent = root;
        symbolTable = table;
    }



    public override object VisitProcedureDivision(CobolParser.ProcedureDivisionContext context)
    {
 
        var cfg = new CobolCFGVisitor().BuildCFG(context);

        //visitor.WriteVariableUsagesToCsv("output.csv");
        new CFGAnalyzer(varToAnalyze: "WS-COUNTER").AnalyzeExecutionFlow(cfg);
        Console.WriteLine(cfg);
        base.VisitProcedureDivision(context);
        return null;
    }



    

    public override object VisitDataDivision(CobolParser.DataDivisionContext context)
    {
        Console.WriteLine("Visiting Program Node");
        base.VisitDataDivision(context);
        //TODO one off on the symbol table
        return null;
    }

    public override object VisitDataDescriptionEntryFormat1(CobolParser.DataDescriptionEntryFormat1Context context)
    {

        if (currentCobolnodeStack.Count != 0)
        {

            var element = currentCobolnodeStack.Pop();
            var levelNumber = element.Level;
            // Determine parent based on level
            while (currentParent.Level >= levelNumber)
            {
                currentParent = currentParent.Parent; // Backtrack to find the correct parent
            }


            currentParent.AddChild(element);
            currentParent = element;
            symbolTable.AddDataNode(element);
        }

        base.VisitDataDescriptionEntryFormat1(context);


        return null;
    }
    public override object VisitLevelNumber(CobolParser.LevelNumberContext context)
    {
        var tempLevelNumber = int.Parse(context.GetText());
        currentCobolnodeStack.Push(new CobolDataVariable() { Level = int.Parse(context.GetText()) });

        currentLevelNumer = tempLevelNumber;
        return base.VisitLevelNumber(context);
    }

    public override object VisitEntryName(CobolParser.EntryNameContext context)
    {
        if (currentCobolnodeStack.Count != 0)
        {
            var element = currentCobolnodeStack.Pop();
            element.Name = context.GetText();
            currentCobolnodeStack.Push(element);
        }

        //Console.WriteLine("Visiting Statement Node: " + context.GetText());

        return base.VisitEntryName(context);
    }

    public override object VisitDataPictureClause(CobolParser.DataPictureClauseContext context)
    {

       // Console.WriteLine("Visiting Statement Node: " + context.GetText());
        return base.VisitDataPictureClause(context);
    }


}
