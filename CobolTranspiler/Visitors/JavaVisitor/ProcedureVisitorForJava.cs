using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CobolTranspiler;

public class ProcedureVisitorForJava : CobolParserBaseVisitor<string>
{

    JavaProcedureLogic javaProcedureLogic = new JavaProcedureLogic();
    Stack<CobolDataVariable> currentCobolnodeStack = new Stack<CobolDataVariable>();

    SymbolTable symbolTable = SymbolTable.Instance;

    bool isInsideSectionOrParagraph = false;

    private StringBuilder programJavaCode; // Program-level StringBuilder

    public override string VisitProcedureDivision([NotNull] CobolParser.ProcedureDivisionContext context)
    {
        programJavaCode = new StringBuilder(); // Initialize program-level StringBuilder here

        // ... (Logic to visit children of procedureDivision, e.g., procedureDivisionBody) ...
        string procedureBodyCode = VisitProcedureDivisionBody(context.procedureDivisionBody());
        if (procedureBodyCode != null)
        {
            programJavaCode.Append(procedureBodyCode); // Append procedure body code
        }

        string finalJavaCode = programJavaCode.ToString(); // Get the complete Java code
        System.Console.WriteLine(finalJavaCode); // Optional: Print to console
        File.WriteAllText("logic.java", finalJavaCode); // Write to file - using WriteAllText to overwrite
        return finalJavaCode; // Return the complete Java code string
    }

    public override string VisitDisplayStatement([NotNull] CobolParser.DisplayStatementContext context)
    {
        return javaProcedureLogic.ParseDisPlayStatement(context);
    }


    public override string VisitSectionOrParagraph([NotNull] CobolParser.SectionOrParagraphContext context)
    {
        return base.VisitSectionOrParagraph(context);
    }

    public override string VisitSentence([NotNull] CobolParser.SentenceContext context)
    {
        StringBuilder javaCode = new StringBuilder();
        foreach (var child in context.children)
        {
            if (child is CobolParser.StatementContext)
            {
                javaCode.AppendLine(Visit(child));
            }
            else
            {
                javaCode.AppendLine(Visit(child));
            }
        }
        System.Console.WriteLine(javaCode);
        File.AppendAllText("logic.java", javaCode.ToString());
        return null;
    }

    public override string VisitStatement([NotNull] CobolParser.StatementContext context)
    {
        foreach (var child in context.children)
        {
            if (child is CobolParser.IfStatementContext)
            {
                Visit(child);
            }
            else if (child is CobolParser.PerformStatementContext)
            {
                Visit(child);
            }
            else if (child is CobolParser.MoveToStatementContext)
            {
                Visit(child);
            }
            else if (child is CobolParser.DisplayStatementContext)
            {
                programJavaCode.Append(Visit(child));
            }
            else
            {
                programJavaCode.Append(Visit(child));

            }
        }
        return null;
    }

    public override string VisitIfStatement([NotNull] CobolParser.IfStatementContext context)
    {
        // Append directly to programJavaCode
        programJavaCode.Append("if (");

        CobolParser.ConditionContext conditionContext = context.condition();
        if (conditionContext != null)
        {
            programJavaCode.Append(Visit(conditionContext));
        }
        programJavaCode.AppendLine(") { ");

        CobolParser.IfThenContext ifThenContext = context.ifThen();
        if (ifThenContext != null)
        {
            VisitIfThen(ifThenContext); // Call VisitIfThen (no need to capture string)
        }

        CobolParser.IfElseContext ifElseContext = context.ifElse();
        if (ifElseContext != null)
        {
            programJavaCode.AppendLine("} else { ");
            VisitIfElse(ifElseContext); // Call VisitIfElse
        }

        programJavaCode.AppendLine("} ");

        return null;
    }

    /* public override string VisitIfThen([NotNull] CobolParser.IfThenContext context)
     {
         StringBuilder thenBlockCode = new StringBuilder();
         // Process statements within the 'THEN' block
         foreach (var statementCall in context.conditionalStatementCall()) // Assuming 'conditionalStatementCall' contains statements
         {
             string statementCode = Visit(statementCall); // Visit each statement
             if (statementCode != null) 
             {
                 thenBlockCode.AppendLine("    " + statementCode); // Indent 'then' block statements
             }
         }
         return thenBlockCode.ToString();
     }*/


    public override string VisitPerformStatement([NotNull] CobolParser.PerformStatementContext context)
    {
        return base.VisitPerformStatement(context);
    }


    public override string VisitIfElse([NotNull] CobolParser.IfElseContext context)
    {
        StringBuilder elseBlockCode = new StringBuilder();
        // Process statements within the 'ELSE' block
        foreach (var statementCall in context.conditionalStatementCall()) // Assuming 'conditionalStatementCall' contains statements
        {
            string statementCode = Visit(statementCall);
            if (statementCode != null) // Ensure statement code is not null
            {
                elseBlockCode.AppendLine("    " + statementCode); // Indent 'else' block statements
            }
        }
        return elseBlockCode.ToString();
    }
    public override string VisitSimpleCondition([NotNull] CobolParser.SimpleConditionContext context)
    {
        StringBuilder condition = new StringBuilder();
        foreach (var child in context.children)
        {
            if (child is CobolParser.ArithmeticExpressionContext)
            {
                var res = Visit(child);
                condition.Append(res);
            }
            else if (child is CobolParser.RelationCombinedComparisonContext)
            {
                var res = Visit(child);
                condition.Append(res);
            }
            else
            {
                condition.AppendLine(Visit(child));
            }
        }
        //base.VisitSimpleCondition(context);
        return condition.ToString();
    }


    public override string VisitRelationCombinedComparison([NotNull] CobolParser.RelationCombinedComparisonContext context)
    {
        StringBuilder condition = new StringBuilder();
        foreach (var child in context.children)
        {
            if (child is CobolParser.RelationalOperatorContext)
            {
                condition.Append(Visit(child));
            }
            else if (child is CobolParser.ArithmeticExpressionContext)
            {
                condition.Append(Visit(child));
            }
            else
            {
                condition.AppendLine(Visit(child));
            }
        }
        return condition.ToString();
    }
    public override string VisitArithmeticExpression([NotNull] CobolParser.ArithmeticExpressionContext context)
    {
        // File.AppendAllText("logic.java", context.GetText() + " ");
        var res = context.GetText() + " ";
        return res;
    }

    public override string VisitRelationalOperator([NotNull] CobolParser.RelationalOperatorContext context)
    {
        //File.AppendAllText("logic.java", context.GetText() + " ");
        return context.GetText() + " ";
    }


    public override string VisitMoveToStatement([NotNull] CobolParser.MoveToStatementContext context)
    {
        programJavaCode.AppendLine(javaProcedureLogic.ParseMoveToStatement(context));
        base.VisitMoveToStatement(context);
        return null;
    }

}