using System.Text.RegularExpressions;

namespace AutoForm.Extensions;

public static class StringHelpers
{
    public static string ToFriendlyCase(this string pascalString)
    {
        return Regex.Replace(pascalString, "(?!^)([A-Z])", " $1");
    }
}