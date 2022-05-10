using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Layoutize.Utils;

public static class TextEditor
{
    public static IEnumerable<string> Format(IEnumerable<string> contents)
    {
        List<string> results = new();
        bool hasPreviousLine = false;
        foreach (string line in contents)
        {
            bool containsText = !string.IsNullOrWhiteSpace(line);
            if (containsText || hasPreviousLine)
            {
                results.Add(Regex.Replace(Regex.Replace(line.TrimEnd(), "(?<=\t) +| +(?=\t)", ""), " {2,}", " "));
                hasPreviousLine = containsText;
            }
        }
        if (results.Any() && string.IsNullOrWhiteSpace(results.Last()))
        {
            results.RemoveAt(results.Count - 1);
        }
        return results;
    }
}
