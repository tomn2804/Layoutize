using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public TextModel(string name)
			: base(name)
		{
			InitializerContents = Array.Empty<string>();
		}

		public TextModel(string name, string[] contents)
			: base(name)
		{
			InitializerContents = contents;
		}

		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}

		private string[] InitializerContents { get; set; }

		public override void Configure()
		{
			base.Configure();
			OnCreate(() => Contents = InitializerContents);
			OnCleanup(() => Contents = Format(Contents));
		}

		public static string[] Format(string[] contents)
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
