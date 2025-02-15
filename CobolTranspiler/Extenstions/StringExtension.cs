using System.Globalization;
using System.Text;

public static class StringExtensions
{
    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input; // Or return string.Empty if you prefer empty string for null/empty input
        }

        // Split the input string by hyphens
        string[] words = input.Split('-');
        StringBuilder camelCaseBuilder = new StringBuilder();

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            if (string.IsNullOrEmpty(word))
            {
                continue; // Skip empty words resulting from consecutive hyphens or leading/trailing hyphens
            }

            // For the first word, convert it to lowercase
            if (i == 0)
            {
                camelCaseBuilder.Append(word.ToLower(CultureInfo.InvariantCulture));
            }
            else
            {
                // For subsequent words, capitalize the first letter and lowercase the rest
                if (word.Length > 0)
                {
                    camelCaseBuilder.Append(char.ToUpper(word[0], CultureInfo.InvariantCulture));
                    if (word.Length > 1)
                    {
                        camelCaseBuilder.Append(word.Substring(1).ToLower(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        return camelCaseBuilder.ToString();
    }
}