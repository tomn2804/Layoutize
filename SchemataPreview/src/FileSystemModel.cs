using Microsoft.VisualBasic.FileIO;

namespace SchemataPreview
{
	public abstract class FileSystemModel : Model
	{
		public FileSystemModel(string name)
			: base(name)
		{
		}

		public static void SendDirectoryToRecycleBin(string path)
		{
			FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public static void SendFileToRecycleBin(string path)
		{
			FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
