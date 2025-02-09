namespace CobolFieldAnalyzer;

public class SymbolTable
{
    private static readonly Lazy<SymbolTable> _instance = new Lazy<SymbolTable>(() => new SymbolTable());

    private readonly Dictionary<string, CobolDataVariable> _dataNodes = new();

    private SymbolTable() { }

    public static SymbolTable Instance => _instance.Value;

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
        return node?.Value;
    }

    public void SetValue(string name, object value)
    {
        var node = GetDataNode(name);
        if (node != null)
        {
            node.Value = value;
        }
    }
}