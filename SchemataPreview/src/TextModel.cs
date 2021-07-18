using System;
using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public override void Build(Builder builder)
		{
			base.Build(builder);
			builder.AddEventListener(EventOption.Create, () =>
			{
				Contents = ContentsToInitialize;
			});
			builder.AddEventListener(EventOption.Cleanup, () =>
			{
				Contents = TextEditor.Format(Model.Contents);
			});
		}

		public string[] ContentsToInitialize = Array.Empty<string>();

		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}
	}
}
