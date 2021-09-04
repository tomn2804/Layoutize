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
			switch (Schema["Contents"])
			{
				case string content:
					Contents = new string[] { content };
					break;

				case string[] contents:
					Contents = contents;
					break;

				default:
					break;
			}
		}
	}
}
