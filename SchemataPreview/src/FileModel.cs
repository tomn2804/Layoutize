using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : FileSystemModel
	{
		public override ModelSet? Children => null;
		public override bool Exists => File.Exists(FullName);

		public virtual void Create()
		{
			File.Create(FullName).Dispose();
		}

		public virtual void Delete()
		{
			FileSystem.DeleteFile(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
