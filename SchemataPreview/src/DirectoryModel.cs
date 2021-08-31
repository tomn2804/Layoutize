using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		public DirectoryModel()
		{
			Children = new(this);
		}

		public override bool Exists => Directory.Exists(FullName);
		public override ModelSet Children { get; }

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
