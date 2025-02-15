
using Antlr4.Runtime.Misc;
using CobolTranspiler;

public class TranspilerForJavaDataDivision : CobolParserBaseVisitor<object>
{
    Stack<CobolDataVariable> currentCobolnodeStack = new Stack<CobolDataVariable>();
    ProcedureVisitorForJava procedureVisitorForJava = new ProcedureVisitorForJava();
    SymbolTable symbolTable = SymbolTable.Instance;
    private readonly CobolParser _parser;

    CobolDataVariable root = new CobolDataVariable() { Level = 0 };
    CobolDataVariable currentParent;
    int currentLevelNumer;

    public TranspilerForJavaDataDivision()
    {
        currentParent = root;
    }
    public override object VisitProcedureDivision([NotNull] CobolParser.ProcedureDivisionContext context)
    {

        return context.Accept(procedureVisitorForJava);
    }

    public override object VisitDataDivisionSection([NotNull] CobolParser.DataDivisionSectionContext context)
    {

        return base.VisitDataDivisionSection(context);
    }

    public override object VisitDot_fs([NotNull] CobolParser.Dot_fsContext context)
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
        return base.VisitDot_fs(context);
    }

    public override object VisitDataDescriptionEntryFormat1(CobolParser.DataDescriptionEntryFormat1Context context)
    {


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

    public override object VisitDataDescriptionEntryFormat3(CobolParser.DataDescriptionEntryFormat3Context context)
    {
        var tempLevelNumber = int.Parse(context.children[0].GetText());
        currentCobolnodeStack.Push(new CobolDataVariable() { Level = tempLevelNumber });

        currentLevelNumer = tempLevelNumber;
        return base.VisitDataDescriptionEntryFormat3(context);
    }

    public override object VisitEntryName(CobolParser.EntryNameContext context)
    {
        if (currentCobolnodeStack.Count != 0)
        {
            var element = currentCobolnodeStack.Pop();
            element.Name = context.GetText();
            currentCobolnodeStack.Push(element);
        }

        return base.VisitEntryName(context);
    }

    public override object VisitDataPictureClause(CobolParser.DataPictureClauseContext context)
    {
        if (currentCobolnodeStack.Count != 0)
        {
            var element = currentCobolnodeStack.Pop();
            element.Type = context.GetText();
            currentCobolnodeStack.Push(element);
        }

        return base.VisitDataPictureClause(context);
    }


    public override object VisitDataValueClauseLiteral(CobolParser.DataValueClauseLiteralContext context)
    {
        if (currentCobolnodeStack.Count != 0)
        {
            var element = currentCobolnodeStack.Pop();
            element.Value = context.GetText();
            currentCobolnodeStack.Push(element);
        }
        return base.VisitDataValueClauseLiteral(context);
    }

}