using Antlr4.Runtime;

namespace DotnetCobolParser;

public class CustomDataDivisionListener : CobolParserBaseListener
{

    private readonly CobolParser _parser;

    public CustomDataDivisionListener(CobolParser parser)
    {
        _parser = parser;
    }

    public override void EnterEveryRule(ParserRuleContext context)
    {
        Console.WriteLine("Entering: " + context.GetText());

    }
}
