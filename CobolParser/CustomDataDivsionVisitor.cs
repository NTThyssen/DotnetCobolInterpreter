namespace DotnetCobolParser;

public class CustomDataDivsionVisitor :CobolParserBaseVisitor<object> // Replace 'Cobol' with your grammar name
{
    public override object VisitDataDivision(CobolParser.DataDivisionContext context)
    {
        Console.WriteLine("Visiting Program Node");
        return base.VisitDataDivision(context);
    }

    public override object VisitDataDescriptionEntryFormat1(CobolParser.DataDescriptionEntryFormat1Context context)
    {
        Console.WriteLine("Visiting Statement Node: " + context.GetText());
        return base.VisitDataDescriptionEntryFormat1(context);
    }

    // Add more methods for other nodes you want to visi

}
