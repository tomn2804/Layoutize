using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public TextModel(ReadOnlySchema schema)
			: base(schema)
		{
			PipeAssembly[PipelineOption.Create].OnProcessing += () =>
			{
				switch (schema["Contents"])
				{
					case string content:
						Contents = new string[] { content };
						break;

					case string[] contents:
						Contents = contents;
						break;
				}
			});
		}

		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}
	}
}
