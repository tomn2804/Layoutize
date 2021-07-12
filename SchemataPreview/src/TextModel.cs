using System;
using System.IO;

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
					Contents = TextEditor.Format(Contents);
				});
			});
		}

		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}

		private string[] InitializerContents { get; set; }
	}
}
