using Microsoft.VisualBasic.FileIO;

namespace SchemataPreview
{
	public static class RecycleBin
	{
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
