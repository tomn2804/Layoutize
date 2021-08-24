using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public override bool Exists => File.Exists(AbsolutePath);
		public override ModelSet? Children => null;

		public virtual void Create()
		{
			File.Create(AbsolutePath).Dispose();
		}

		public virtual void Delete()
		{
			FileSystem.DeleteFile(AbsolutePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
