using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		protected void OnCreate()
		{
			File.Create(this).Dispose();
		}

		protected void OnDelete()
		{
			FileSystem.DeleteFile(this, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
