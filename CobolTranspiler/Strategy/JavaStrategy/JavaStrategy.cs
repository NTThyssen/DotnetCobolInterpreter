namespace CobolTranspiler.Strategy.JavaStrategy;

public class JavaStrategy : IStrategy
{
    public void Process(string path)
    {
        JavaDataStructure javaDataStructure = new JavaDataStructure();
        javaDataStructure.CreateJavaDataStruktures();
    }

    public string TranspileDataStructure(string dataStructure)
    {
        throw new NotImplementedException();
    }
}