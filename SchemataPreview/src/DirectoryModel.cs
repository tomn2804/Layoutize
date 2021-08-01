using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		protected void OnCreate()
		{
			Directory.CreateDirectory(this);
		}

		protected void OnDelete()
		{
			FileSystem.DeleteDirectory(this, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
