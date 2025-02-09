namespace CobolFieldAnalyzer;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;



// Make sure to replace 'YourCobolGrammar' with your actual grammar name!
public class CobolCFGVisitor : CobolParserBaseVisitor<object> // or IParseTreeVisitor<object> if you are using interface visitor
{
    private ControlFlowGraph cfg = new ControlFlowGraph();
    private CFGNode currentParagraphNode = null;
    private Dictionary<string, CFGNode> paragraphNodes = new Dictionary<string, CFGNode>();
    private List<(string from, string to)> pendingEdges = new List<(string, string)>(); // Store pending edges


    public ControlFlowGraph BuildCFG(CobolParser.ProcedureDivisionContext context)
    {
        VisitProcedureDivision(context);

        // Resolve pending edges after all nodes are visited
        foreach (var edge in pendingEdges)
        {
            cfg.AddEdge(edge.from, edge.to); // Now AddEdge should work without warnings
        }

        return cfg;
    }

    public override object VisitProcedureDivision([NotNull] CobolParser.ProcedureDivisionContext context)
    {
        CFGNode mainParagraphNode = new CFGNode("MAIN");
        cfg.AddNode(mainParagraphNode);
        paragraphNodes["MAIN"] = mainParagraphNode;
        currentParagraphNode = mainParagraphNode;

        Console.WriteLine($"DEBUG: Created MAIN node: {mainParagraphNode}, Nodes count: {cfg.Nodes.Count}");

        base.VisitProcedureDivision(context);

        return null;
    }


    public override object VisitSectionOrParagraph([NotNull] CobolParser.SectionOrParagraphContext context)
    {
        string paragraphName = GetParagraphName(context);

        if (!string.IsNullOrEmpty(paragraphName))
        {
            CFGNode newNode = new CFGNode(paragraphName);
            cfg.AddNode(newNode);
            paragraphNodes[paragraphName] = newNode;
            currentParagraphNode = newNode;
            currentParagraphNode.StatementContexts.Add(context); // Store the parse tree context
            Console.WriteLine($"DEBUG: Created paragraph node: {newNode}, Nodes count: {cfg.Nodes.Count}");
        }

        return base.VisitSectionOrParagraph(context);
    }



    public override object VisitSentence([NotNull] CobolParser.SentenceContext context)
    {

        var a = context.GetChild(0).GetText();
        if (currentParagraphNode != null && !context.GetChild(0).GetText().StartsWith("PERFORM"))
        {

            currentParagraphNode.StatementContexts.Add(context);
        }

        return base.VisitSentence(context);
    }


    public override object VisitPerformStatement([NotNull] CobolParser.PerformStatementContext context)
    {
        if (currentParagraphNode != null)
        {
            currentParagraphNode.StatementContexts.Add(context);
            string targetParagraphName = GetPerformTargetParagraphName(context);

            if (!string.IsNullOrEmpty(targetParagraphName))
            {
                pendingEdges.Add((currentParagraphNode.ParagraphName, targetParagraphName)); // Add to pending edges
            }
            else
            {
                Console.WriteLine($"Warning: PERFORM target paragraph name could not be extracted.");
            }
        }
        else
        {
            Console.WriteLine("Warning: PERFORM statement found outside of a paragraph context.");
        }
        return base.VisitPerformStatement(context);
    }


    // --- Helper Methods --- (Adjust these based on your actual grammar)

    private string GetParagraphName(CobolParser.SectionOrParagraphContext context)
    {
        if (context.cobolWord() != null)
        {
            return context.cobolWord().GetText();
        }
        if (context.integerLiteral() != null)
        {
            return context.integerLiteral().ToString();
        }
        return null;
    }

    private string GetPerformTargetParagraphName(CobolParser.PerformStatementContext context) // Adjust context type
    {
        // **Adapt this based on your actual grammar for PERFORM statements.**
        // For our assumed grammar: `performStatement : PERFORM paragraphName=identifier;`
        if (context.children[1].GetChild(0).GetText() != null)
        {
            return context.children[1].GetChild(0).GetText();
        }
        return null; // Or handle error/logging if target name is not found
    }
}
