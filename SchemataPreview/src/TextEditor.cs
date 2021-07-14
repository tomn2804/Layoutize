using System.Collections.Generic;
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
			return results.ToArray();
		}
	}
}
