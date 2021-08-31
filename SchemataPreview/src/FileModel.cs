using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public override bool Exists => File.Exists(FullName);
		public override ModelSet? Children => null;

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
