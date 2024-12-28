using Antlr4.Runtime.Tree;

namespace DotnetCobolParser;

public class CustomDataDivsionVisitor : CobolParserBaseVisitor<object> // Replace 'Cobol' with your grammar name
{

    Stack<CobolDataVariable> currentCobolnodeStack = new Stack<CobolDataVariable>();

    SymbolTable symbolTable = new SymbolTable();
    private readonly CobolParser _parser;

    CobolDataVariable root = new CobolDataVariable() { Level = 0 };
    CobolDataVariable currentParent;
    int currentLevelNumer;
    public CustomDataDivsionVisitor(CobolParser parser)
    {
        _parser = parser;
        currentParent = root;
    }


    public override object VisitProcedureDivision(CobolParser.ProcedureDivisionContext context)
    {
        Console.WriteLine("Visiting Program procedure");
        base.VisitProcedureDivision(context);
        return null;
    }

    public override object VisitMoveToStatement(CobolParser.MoveToStatementContext context)
    {
        var source = context.children[0];
        var destination = context.children[2];
        if (source.GetChild(0) is CobolParser.LiteralContext)
        {
            symbolTable.SetValue(destination.GetText(), source.GetText());
        }
        else
        {
            symbolTable.SetValue(destination.GetText(), symbolTable.GetValue(source.GetText()));
        }
        base.VisitMoveToStatement(context);
        return null;
    }



    public override object VisitDisplayOperand(CobolParser.DisplayOperandContext context)
    {
        if (context.GetChild(0) is CobolParser.LiteralContext)
        {
            Console.WriteLine(context.GetText());
        }
        else
        {
            Console.WriteLine(symbolTable.GetValue(context.GetText()));
        }
        base.VisitDisplayOperand(context);
        return null;
    }

    public override object VisitIfStatement(CobolParser.IfStatementContext context)
    {
        var boolresult = EvaluateCondition(context.GetChild(1).GetChild(0) as CobolParser.SimpleConditionContext);
        // 2. If it's true, visit the THEN part
        if (boolresult)
        {
            Visit(context.ifThen());
        }
        // 3. Otherwise, if there's an ELSE part, visit that
        else if (context.ifElse() != null)
        {
            Visit(context.ifElse());
        }
        return null;
    }


    private bool EvaluateCondition(CobolParser.SimpleConditionContext conditionCtx)
    {
        var t = conditionCtx.GetChild(0).GetText();
        var left = symbolTable.GetValue(conditionCtx.GetChild(0).GetText()) == null ? conditionCtx.GetChild(0).GetText() : symbolTable.GetValue(conditionCtx.GetChild(0).GetText());
        return EvaluateReleationCombined(left, conditionCtx.relationCombinedComparison());
    }

    private bool EvaluateReleationCombined(object left, CobolParser.RelationCombinedComparisonContext context)
    {

        var leftNum = left is string ? int.Parse(left.ToString()) : (int)left;
        var rightNum = symbolTable.GetValue(context.arithmeticExpression()[0].GetText()) == null ? int.Parse(context.arithmeticExpression()[0].GetText()) : int.Parse(symbolTable.GetValue(context.arithmeticExpression()[0].GetText()).ToString());
        var op = context.relationalOperator().GetText();
        switch (op)
        {
            case "=":
            case "EQUAL": // sometimes COBOL uses words
                return leftNum == rightNum;
            case "<":
            case "LESS":
                return leftNum < rightNum;
            case ">":
            case "GREATER":
                return leftNum > rightNum;
            case "<=":
            case "NOT GREATER":
                // Some COBOL dialects have synonyms for <= or >=
                return leftNum <= rightNum;
            case ">=":
            case "NOT LESS":
                return leftNum >= rightNum;
            case "<>":
            case "NOT =":
                return leftNum != rightNum;
            default:
                // If there's some unrecognized operator, handle accordingly
                throw new Exception($"Unknown relational operator: {op}");
        }

    }


    public override object VisitDataDivision(CobolParser.DataDivisionContext context)
    {
        Console.WriteLine("Visiting Program Node");
        base.VisitDataDivision(context);
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



    // Add more methods for other nodes you want to visi

}
