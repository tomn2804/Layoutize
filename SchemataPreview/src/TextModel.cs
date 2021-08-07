using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public override void Create()
		{
			base.Create();
			Contents = (string[])Schema["Contents"];
		}

		public string[] Contents
		{
			get => File.ReadAllLines(AbsolutePath);
			set => File.WriteAllLines(AbsolutePath, value);
		}
	}
}
