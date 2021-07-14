using System;
using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public override void PresetConfiguration()
		{
			base.PresetConfiguration();
			AddEventListener(EventOption.Create, () =>
			{
				Contents = ContentsToInitialize;
			});
			AddEventListener(EventOption.Cleanup, () =>
			{
				Contents = TextEditor.Format(Contents);
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
