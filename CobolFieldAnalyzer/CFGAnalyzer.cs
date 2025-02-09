using Antlr4.Runtime.Tree;
using CobolFieldAnalyzer;

public class CFGAnalyzer{

    private ControlFlowGraph _cfg; // Renamed to _cfg to distinguish from parameter

    CustomCobolVisitor customCobolVisitor;
    public CFGAnalyzer(string varToAnalyze)
    {
  
        customCobolVisitor = new CustomCobolVisitor(varToAnalyze, SymbolTable.Instance);
    }
    public void AnalyzeExecutionFlow(ControlFlowGraph cfg) // cfg is now a parameter passed to AnalyzeExecutionFlow
    {
       
         _cfg = cfg; // Assign the passed cfg to the class field _cfg
        HashSet<CFGNode> visitedNodes = new HashSet<CFGNode>();
        CFGNode startNode = _cfg.Nodes.ContainsKey("MAIN") ? _cfg.Nodes["MAIN"] : _cfg.Nodes.Values.FirstOrDefault();

        if (startNode != null)
        {
            Console.WriteLine("Starting Statement Execution Flow Analysis:");
            ExecuteStatementsAndPerformCalls(startNode, visitedNodes); // Call the new traversal function
        }
        else
        {
            Console.WriteLine("Warning: No starting node (MAIN or any node) found in CFG to start analysis.");
        }
    }

    private void ExecuteStatementsAndPerformCalls(CFGNode currentNode, HashSet<CFGNode> visited)
    {
        if (visited.Contains(currentNode))
        {
            Console.WriteLine($"  Detected cycle, already visited node: {currentNode.ParagraphName} - returning to prevent infinite recursion.");
            return; // Already visited, prevent cycles
        }
        visited.Add(currentNode);

        Console.WriteLine($"\nExecuting statements in Node: {currentNode.ParagraphName}");

        foreach (IParseTree statementContext in currentNode.StatementContexts)
        {
            Console.WriteLine($"  Executing Statement: {statementContext.GetText()}");
            customCobolVisitor.Visit(statementContext); // Visit the statement with your custom visitor
            // --- Placeholder for Variable Access Analysis per statement ---
            // Implement your variable read/write detection and state updates here
            // For now, just a placeholder message:
            // Console.WriteLine("    [Variable Analysis Placeholder - Implement Read/Write detection here]");


            // Check if the statement is a PERFORM statement
            if (statementContext is CobolParser.PerformStatementContext performStmtCtx) // Adjust context type
            {
                string targetParagraphName = GetPerformTargetParagraphName(performStmtCtx); // Helper function

                if (!string.IsNullOrEmpty(targetParagraphName) && _cfg.Nodes.ContainsKey(targetParagraphName)) // **Use _cfg here**
                {
                    CFGNode targetNode = _cfg.Nodes[targetParagraphName]; // **Use _cfg here**
                    Console.WriteLine($"    PERFORM call to paragraph: {targetParagraphName}");
                    ExecuteStatementsAndPerformCalls(targetNode, visited); // Recursive call for PERFORMED paragraph
                    Console.WriteLine($"    Returned from PERFORM call to: {targetParagraphName} - continuing in {currentNode.ParagraphName}"); // Indicate return
                }
                else
                {
                    Console.WriteLine($"    Warning: PERFORM target paragraph '{targetParagraphName}' not found in CFG.");
                }
            }
            // --- Continue to the next statement in the current node after PERFORM (or any other statement) ---
        }

        // After processing all statements in the current node, you could handle implicit fall-through to successors if needed
        // In this example, explicit PERFORM calls are the primary control flow mechanism.
        // If you have implicit fall-through from one paragraph to the next sequentially defined paragraph, you'd handle that here
        // by iterating through currentNode.Successors if they represent implicit flow, not just PERFORM calls.

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