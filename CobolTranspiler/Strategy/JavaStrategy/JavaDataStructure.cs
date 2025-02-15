namespace CobolTranspiler.Strategy.JavaStrategy;

public class JavaDataStructure
{

    SymbolTable symbolTable = SymbolTable.Instance;


    public void CreateJavaDataStruktures()
    {

        foreach (var dataNode in symbolTable.dataNodes.Values)
        {
            if (dataNode.Children.Count > 0)
            {
                CreateClass(dataNode);
            }
        }
    }

    public string createEnumHelperFuntions(string enumNameRaw)
    {
        var enumName = UppercaseFirst(enumNameRaw);
        string output = "";
        output += "private final String code;  \n\n";

        output += enumName + "(String code) { \n";
        output += "this.code = code; \n";
        output += "} \n\n";

        output += "public String getCode() { \n";
        output += "return code; \n";
        output += "} \n\n";

        output += "public static " + enumName + " fromCode(String code) { \n";
        output += "for (" + enumName + " e : " + enumName + ".values()) { \n";
        output += "if (e.code.equals(code)) { \n";
        output += "return e; \n";
        output += "} \n";
        output += "} \n";
        output += "return null; \n";
        output += "} \n";
        return output;


    }
    public void CreateClass(CobolDataVariable dataNode)
    {
        var className = dataNode.Name;
        string? classStringOrEnumString;
        if (dataNode.Children[0].Level == 88)
        {
            classStringOrEnumString = "public enum " + UppercaseFirst(className.ToCamelCase()) + " { \n";
            for (int i = 0; i < dataNode.Children.Count; i++)
            {
                if (i == dataNode.Children.Count - 1)
                {
                    classStringOrEnumString += dataNode.Children[i].Name.Replace("-", "_")
                    + "(" + dataNode.Children[i].Value?.ToString().Replace("'", "\"")
                    + ");\n";
                }
                else
                {
                    classStringOrEnumString += dataNode.Children[i].Name.Replace("-", "_")
                    + "(" + dataNode.Children[i].Value.ToString().Replace("'", "\"")
                    + "),\n";

                }
            }
            classStringOrEnumString += createEnumHelperFuntions(className.ToCamelCase());
        }
        else
        {
            classStringOrEnumString = "public class " + UppercaseFirst(className.ToCamelCase()) + " { \n";

            foreach (var child in dataNode.Children)
            {
                classStringOrEnumString += "public " + ParsePicType(child) + " " + child.Name.ToCamelCase() + parsePicValue(child) + ";\n";
            }
        }
        classStringOrEnumString += "}";
        // File.WriteAllText(className.ToCamelCase() + ".java", classStringOrEnumString);
    }

    private string parsePicValue(CobolDataVariable child)
    {
        var picValue = child.Value?.ToString();
        if (picValue == null)
        {
            return "";
        }

        return " = " + picValue;
    }
    private string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    private string ParsePicType(CobolDataVariable child)
    {
        var picType = child.Type;
        if (picType == null)
        {
            return UppercaseFirst(child.Name.ToCamelCase());
        }
        if (picType.Contains("X"))
        {
            return "String";
        }
        else if (picType.Contains("9") && !picType.Contains("V9"))
        {
            return "int";
        }
        else if (picType.Contains("V9"))
        {
            return "double";
        }
        else
        {
            return "String";
        }
    }

}
