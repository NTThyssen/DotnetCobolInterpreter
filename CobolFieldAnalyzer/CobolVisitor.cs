namespace CobolFieldAnalyzer;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotnetCobolParser;


public class CustomCobolVisitor : CobolParserBaseVisitor<object> // Replace 'Cobol' with your grammar name
{

    string variableToTrack;

    SymbolTable symbolTable = SymbolTable.Instance;
    public CustomCobolVisitor(string input, SymbolTable table)
    {
        variableToTrack = input;
        symbolTable = table;
    }


    //create a function which goes though the symbol table and finds if any of the current variable's parents are in the symbol table
    public bool IsParentInSymbolTable(string varToTrack, string variableName)
    {
        CobolDataVariable currentVariable = symbolTable.GetDataNode(varToTrack);
        while (currentVariable.Parent != null)
        {
            if (currentVariable.Parent == symbolTable.GetDataNode(variableName))
            {
                return true;
            }
            currentVariable = currentVariable.Parent;
        }
        return false;
    }
    public CobolDataVariable getParent(string varToTrack, string variableName)
    {
        CobolDataVariable currentVariable = symbolTable.GetDataNode(varToTrack);
        while (currentVariable.Parent != null)
        {
            if (currentVariable.Parent == symbolTable.GetDataNode(variableName))
            {
                return currentVariable.Parent;
            }
            currentVariable = currentVariable.Parent;
        }
        return null;
    }

    private List<(string StatementType, string VariableName, int LineNumber)> variableUsages = new List<(string StatementType, string VariableName, int LineNumber)>();

    public override object VisitVariableUsageName(CobolParser.VariableUsageNameContext context)
    {
        string variableName = context.GetText();
        if (variableName != variableToTrack && !IsParentInSymbolTable(variableToTrack, variableName))
        {
            return base.VisitVariableUsageName(context);
        }

        string statementType = "Unknown Statement";
        int lineNumber = context.Start.Line; // Get the line number

        // Walk up the tree to find the parent statement
        IParseTree currentNode = context.Parent;
        while (currentNode != null)
        {
            if (currentNode is CobolParser.StatementContext statementContext)
            {
                // Determine the type of statement
                if (statementContext.acceptStatement() != null) statementType = "ACCEPT";
                else if (statementContext.addStatement() != null) statementType = "ADD";
                else if (statementContext.allocateStatement() != null) statementType = "ALLOCATE";
                else if (statementContext.alterStatement() != null) statementType = "ALTER";
                else if (statementContext.callStatement() != null) statementType = "CALL";
                else if (statementContext.cancelStatement() != null) statementType = "CANCEL";
                else if (statementContext.closeStatement() != null) statementType = "CLOSE";
                else if (statementContext.computeStatement() != null) statementType = "COMPUTE";
                else if (statementContext.continueStatement() != null) statementType = "CONTINUE";
                else if (statementContext.deleteStatement() != null) statementType = "DELETE";
                else if (statementContext.disableStatement() != null) statementType = "DISABLE";
                else if (statementContext.displayStatement() != null) statementType = "DISPLAY";
                else if (statementContext.divideStatement() != null) statementType = "DIVIDE";
                else if (statementContext.enableStatement() != null) statementType = "ENABLE";
                else if (statementContext.entryStatement() != null) statementType = "ENTRY";
                else if (statementContext.evaluateStatement() != null) statementType = "EVALUATE";
                else if (statementContext.exhibitStatement() != null) statementType = "EXHIBIT";
                else if (statementContext.exitStatement() != null) statementType = "EXIT";
                else if (statementContext.freeStatement() != null) statementType = "FREE";
                else if (statementContext.generateStatement() != null) statementType = "GENERATE";
                else if (statementContext.gobackStatement() != null) statementType = "GOBACK";
                else if (statementContext.goToStatement() != null) statementType = "GO TO";
                else if (statementContext.ifStatement() != null) statementType = "IF";
                else if (statementContext.initializeStatement() != null) statementType = "INITIALIZE";
                else if (statementContext.initiateStatement() != null) statementType = "INITIATE";
                else if (statementContext.inspectStatement() != null) statementType = "INSPECT";
                else if (statementContext.mergeStatement() != null) statementType = "MERGE";
                else if (statementContext.moveStatement() != null) statementType = "MOVE";
                else if (statementContext.multiplyStatement() != null) statementType = "MULTIPLY";
                else if (statementContext.openStatement() != null) statementType = "OPEN";
                else if (statementContext.performStatement() != null) statementType = "PERFORM";
                else if (statementContext.purgeStatement() != null) statementType = "PURGE";
                else if (statementContext.readStatement() != null) statementType = "READ";
                else if (statementContext.readyResetTraceStatement() != null) statementType = "READY/RESET TRACE";
                else if (statementContext.receiveStatement() != null) statementType = "RECEIVE";
                else if (statementContext.releaseStatement() != null) statementType = "RELEASE";
                else if (statementContext.returnStatement() != null) statementType = "RETURN";
                else if (statementContext.rewriteStatement() != null) statementType = "REWRITE";
                else if (statementContext.searchStatement() != null) statementType = "SEARCH";
                else if (statementContext.sendStatement() != null) statementType = "SEND";
                else if (statementContext.serviceStatement() != null) statementType = "SERVICE RELOAD";
                else if (statementContext.setStatement() != null) statementType = "SET";
                else if (statementContext.sortStatement() != null) statementType = "SORT";
                else if (statementContext.startStatement() != null) statementType = "START";
                else if (statementContext.stopStatement() != null) statementType = "STOP";
                else if (statementContext.stringStatement() != null) statementType = "STRING";
                else if (statementContext.subtractStatement() != null) statementType = "SUBTRACT";
                else if (statementContext.terminateStatement() != null) statementType = "TERMINATE";
                else if (statementContext.unstringStatement() != null) statementType = "UNSTRING";
                else if (statementContext.writeStatement() != null) statementType = "WRITE";
                else if (statementContext.xmlStatement() != null) statementType = "XML PARSE";
                else if (statementContext.jsonStatement() != null) statementType = "JSON STATEMENT"; // Assuming this is your custom extension

                Console.WriteLine($"Variable '{variableName}' used in {statementType} statement at line {lineNumber}."); // Added line number to console output
                variableUsages.Add((statementType, variableName, lineNumber)); // Store line number
                break; // Stop walking up once we find a statement
            }
            currentNode = currentNode.Parent;
        }
        if (statementType == "Unknown Statement")
        {
            Console.WriteLine($"Variable '{variableName}' usage context not found within a known statement at line {lineNumber}."); // Added line number to console output
            variableUsages.Add((statementType, variableName, lineNumber)); // Still record it, maybe for data division or other contexts
        }

        return base.VisitVariableUsageName(context);
    }

    public void WriteVariableUsagesToCsv(string filePath = "variable_usage.csv")
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Statement Type,Variable Name,Line Number"); // CSV Header - Added Line Number
                foreach (var usage in variableUsages)
                {
                    writer.WriteLine($"{usage.StatementType},{usage.VariableName},{usage.LineNumber}"); // Write line number to CSV
                }
            }
            Console.WriteLine($"Variable usage information written to '{filePath}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing variable usage to CSV file: {ex.Message}");
        }
    }
}