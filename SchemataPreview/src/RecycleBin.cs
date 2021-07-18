using Microsoft.VisualBasic.FileIO;

namespace SchemataPreview
{
	public static class RecycleBin
	{
		public static void DeleteDirectory(string path)
		{
			FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public static void DeleteFile(string path)
		{
			FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
