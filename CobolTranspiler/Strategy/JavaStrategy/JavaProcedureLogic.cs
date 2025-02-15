using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class JavaProcedureLogic
{

    string assignFromVariable = "";
    public void ParseMoveToSendingArea(CobolParser.MoveToSendingAreaContext context)
    {
        IParseTree sourceChild = context.GetChild(0);

        if (HasVariableUsageNameDescendant(sourceChild))
        {
            assignFromVariable = sourceChild.GetText();
        }
        else
        {
            assignFromVariable = sourceChild.GetText();
        }
    }
    public string ParseMoveToStatement(CobolParser.MoveToStatementContext context)
    {
        var assignToVariable = "";
        ParseMoveToSendingArea(context.moveToSendingArea());
        int toNodeIndex = -1;
        for (int i = 0; i < context.ChildCount; i++)
        {
            var node = context.GetChild(i);

            if (toNodeIndex != -1 && i > toNodeIndex)
            {
                if (toNodeIndex <= context.ChildCount - 1)
                {

                    for (int j = i; j < context.ChildCount; j++)
                    {
                        var t = context.GetChild(j).GetText();
                        assignToVariable += context.GetChild(j).GetText() + " = " + assignFromVariable + ";\n";
                    }
                    //assignToVariable = node.GetText() + " = " + assignToVariable;
                    break;
                }
                else
                {
                    assignToVariable = node.GetText() + " = " + assignFromVariable + ";\n"; ;
                }

            }
            if (node is ITerminalNode && node.GetText() == "TO")
            {
                toNodeIndex = i;
            }
        }

        return assignToVariable;
    }



    public string ParseDisPlayStatement(CobolParser.DisplayStatementContext context)
    {
        string displayVariable = "";
        for (int i = 0; i < context.ChildCount; i++)
        {
            var node = context.GetChild(i);
            if (node is CobolParser.DisplayOperandContext)
            {
                displayVariable += node.GetText() + " ";
            }
        }
        // File.AppendAllText("logic.java", "System.out.println(" + displayVariable + ");\n");
        return "System.out.println(" + displayVariable + ");\n";
    }

    private bool HasVariableUsageNameDescendant(IParseTree node)
    {

        if (node is CobolParser.VariableUsageNameContext)
        {
            return true;
        }

        // Recursive step: Check all children
        if (node is IRuleNode ruleNode)
        {
            RuleContext ruleContext = (RuleContext)ruleNode;
            for (int i = 0; i < ruleContext.ChildCount; i++)
            {
                if (HasVariableUsageNameDescendant(ruleContext.GetChild(i)))
                {
                    return true;
                }
            }
        }
        return false;
    }

}