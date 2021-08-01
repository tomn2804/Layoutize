using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public static class FileExtension
	{
		public static void SendItemToRecycleBin(string path)
		{
			if (Directory.Exists(path))
			{
				SendDirectoryToRecycleBin(path);
			}
			else
			{
				SendFileToRecycleBin(path);
			}
		}

		public static void SendDirectoryToRecycleBin(string path)
		{
			Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public static void SendFileToRecycleBin(string path)
		{
			Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public static void ValidateFileNameChars(string name)
		{
			if (string.IsNullOrWhiteSpace(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				throw;
			}
		}

		public static void ValidatePathChars(string name)
		{
			if (string.IsNullOrWhiteSpace(name) || name.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
			{
				throw;
			}
		}
	}
}
