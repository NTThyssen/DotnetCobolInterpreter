using Antlr4.Runtime.Tree;

namespace DotnetCobolParser;

public class CustomDataDivsionVisitor : CobolParserBaseVisitor<object> // Replace 'Cobol' with your grammar name
{

    Stack<CobolDataVariable> globalCobolnodeStack = new Stack<CobolDataVariable>();
    Stack<CobolDataVariable> currentCobolnodeStack = new Stack<CobolDataVariable>();
    private readonly CobolParser _parser;

    CobolDataVariable root = new CobolDataVariable(){Level = 0};
    CobolDataVariable currentParent;
    public CustomDataDivsionVisitor(CobolParser parser)
    {
        _parser = parser;
        currentParent = root;
    }
    int currentLevelNumer = 0;
    public override object VisitDataDivision(CobolParser.DataDivisionContext context)
    {
        Console.WriteLine("Visiting Program Node");
        base.VisitDataDivision(context);
        return null;
    }

    public override object VisitDataDescriptionEntryFormat1(CobolParser.DataDescriptionEntryFormat1Context context)
    {

        if(currentCobolnodeStack.Count != 0){

            var element = currentCobolnodeStack.Pop();
            var levelNumber = element.Level;
            // Determine parent based on level
            while (currentParent.Level >= levelNumber)
            {
                currentParent = currentParent.Parent; // Backtrack to find the correct parent
            }

        
            currentParent.AddChild(element);
            currentParent = element;
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

        Console.WriteLine("Visiting Statement Node: " + context.GetText());

        return base.VisitEntryName(context);
    }

    public override object VisitDataPictureClause(CobolParser.DataPictureClauseContext context)
    {

        Console.WriteLine("Visiting Statement Node: " + context.GetText());
        return base.VisitDataPictureClause(context);
    }



    // Add more methods for other nodes you want to visi

}
