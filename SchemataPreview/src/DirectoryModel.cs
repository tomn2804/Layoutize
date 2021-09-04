using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : FileSystemModel
	{
		public DirectoryModel()
		{
			Children = new(this);
		}

		public override ModelSet Children { get; }
		public override bool Exists => Directory.Exists(FullName);

		public virtual void Create()
		{
			Directory.CreateDirectory(FullName);
		}

		public virtual void Delete()
		{
			FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
