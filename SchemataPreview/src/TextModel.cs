using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public string[] Contents
		{
			get => File.ReadAllLines(AbsolutePath);
			set => File.WriteAllLines(AbsolutePath, value);
		}

		public override void Create()
		{
			base.Create();
			Contents = Schema["Contents"];
		}
	}
}
