
public class LanguageProcessor
{

    IStrategy strategy;
    public LanguageProcessor(IStrategy strategy)
    {
        this.strategy = strategy;
    }
    
    public void Process(string path)
    {
        strategy.Process(path);
    }
}
