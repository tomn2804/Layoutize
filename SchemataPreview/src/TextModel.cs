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
				schema.TryGetValue("Contents", out object? contents);
				switch (contents)
				{
					case string line:
						Contents = new string[] { line };
						break;

					case string[] lines:
						Contents = lines;
						break;
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
