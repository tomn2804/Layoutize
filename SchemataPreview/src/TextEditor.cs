using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchemataPreview
{
	public static class TextEditor
	{
		public static string[] Format(IEnumerable<string> contents)
		{
			List<string> results = new();
			bool hasPreviousLine = false;
			foreach (string line in contents)
			{
				if (!string.IsNullOrWhiteSpace(line) || hasPreviousLine)
				{
					results.Add(Regex.Replace(Regex.Replace(line.TrimEnd(), "(?<=\t) +| +(?=\t)", ""), " {2,}", " "));
					hasPreviousLine = !hasPreviousLine;
				}
			}
			if (results.Any() && string.IsNullOrWhiteSpace(results.Last()))
			{
				results.RemoveAt(results.Count - 1);
			}
			return results.ToArray();
		}
	}
}
