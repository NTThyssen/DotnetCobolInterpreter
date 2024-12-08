using DotnetCobolParser;

public class SymbolTable
{
    private readonly Dictionary<string, CobolDataVariable> _dataNodes = new();

    public void AddDataNode(CobolDataVariable node)
    {
        _dataNodes[node.Name] = node;
    }

    public CobolDataVariable GetDataNode(string name)
    {
        return _dataNodes.TryGetValue(name, out var node) ? node : null;
    }

    public object GetValue(string name)
    {
        var node = GetDataNode(name);
        return node.Value; // You might store a separate 'Value' property or another structure for actual data values
    }

    public void SetValue(string name, List<CobolDataVariable> value)
    {
        var node = GetDataNode(name);
        if (node != null)
        {
            node.Value = value; // Assign the value to the variable for later use
        }
    }
}