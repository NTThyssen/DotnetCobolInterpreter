using System.Collections.Generic;
using Antlr4.Runtime.Tree;

public class CFGNode
{
    public string ParagraphName { get; set; }
    public List<CFGNode> Successors { get; set; } = new List<CFGNode>();

    // New property to store statement parse tree contexts
    public List<IParseTree> StatementContexts { get; set; } = new List<IParseTree>();

    public CFGNode(string paragraphName)
    {
        ParagraphName = paragraphName;
    }

    public override string ToString()
    {
        return $"Node: {ParagraphName}";
    }
}

public class ControlFlowGraph
{
    public Dictionary<string, CFGNode> Nodes { get; } = new Dictionary<string, CFGNode>();

    public void AddNode(CFGNode node)
    {
        Nodes[node.ParagraphName] = node;
    }

    public void AddEdge(string fromParagraph, string toParagraph)
    {
        if (Nodes.ContainsKey(fromParagraph) && Nodes.ContainsKey(toParagraph))
        {
            Nodes[fromParagraph].Successors.Add(Nodes[toParagraph]);
        }
        else
        {
            // Handle error: Paragraph not found (optional error handling)
            Console.WriteLine($"Warning: Paragraph '{fromParagraph}' or '{toParagraph}' not found in CFG nodes.");
        }
    }

    public override string ToString()
    {
        string graphString = "Control Flow Graph:\n";
        foreach (var node in Nodes.Values)
        {
            graphString += $"{node} -> [";
            foreach (var successor in node.Successors)
            {
                graphString += $"{successor.ParagraphName}, ";
            }
            graphString = graphString.TrimEnd(',', ' ') + "]\n";
        }
        return graphString;
    }
}