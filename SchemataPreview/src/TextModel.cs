using System.IO;

namespace SchemataPreview
{
	public class TextModel : FileModel
	{
		public string[] Contents
		{
			get => File.ReadAllLines(FullName);
			set => File.WriteAllLines(FullName, value);
		}

		public override void Create()
		{
			base.Create();
			Contents = Schema["Contents"];
		}
	}
}
