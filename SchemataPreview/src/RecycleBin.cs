using Microsoft.VisualBasic.FileIO;

namespace SchemataPreview
{
	public static class RecycleBin
	{
		public static void RemoveDirectory(string path)
		{
			FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public static void RemoveFile(string path)
		{
			FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
