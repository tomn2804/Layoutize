﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public TextModel(string name)
			: this(name, Array.Empty<string>())
		{
		}

		public TextModel(string name, string[] contents)
			: base(name)
		{
			InitializerContents = contents;
			Configure(() =>
			{
				AddEventListener(EventOption.Create, () =>
				{
					Contents = InitializerContents;
				});
				AddEventListener(EventOption.Cleanup, () =>
				{
					Contents = Format(Contents);
				});
			});
		}

		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}

		private string[] InitializerContents { get; set; }

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
