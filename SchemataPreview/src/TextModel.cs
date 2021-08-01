using System.IO;

namespace SchemataPreview
{
	public class TextModel : Model<FileModel>
	{
		protected void OnCreate()
		{
			Contents = (string[])Schema["Contents"];
		}

		public string[] Contents
		{
			get => File.ReadAllLines(this);
			set => File.WriteAllLines(this, value);
		}
	}
}
