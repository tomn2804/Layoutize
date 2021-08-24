using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		public override bool Exists => Directory.Exists(AbsolutePath);
		public override ModelSet Children { get; } = new();

		public virtual void Create()
		{
			Directory.CreateDirectory(AbsolutePath);
		}

		public virtual void Delete()
		{
			FileSystem.DeleteDirectory(AbsolutePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
