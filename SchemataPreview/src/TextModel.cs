using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public TextModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly[PipeOption.Create].OnProcessing += (_, _) =>
			{
				if (schema.TryGetValue("Contents", out object? contents))
				{
					Contents = contents.ToArray<string>();
				}
			};
		}

		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}
	}
}
