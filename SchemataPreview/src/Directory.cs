using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace SchemataPreview.Models
{
	public class Directory : Model
	{
		public Directory(string name)
			: base(name)
		{
		}

		public override void Create()
		{
			System.IO.Directory.CreateDirectory(FullName);
		}

		public override void Delete()
		{
			FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public override bool Exists()
		{
			return System.IO.Directory.Exists(FullName);
		}

		public override void Cleanup()
		{
			CleanupSubDirectories();
			CleanupSubFiles();
		}

		private void CleanupSubDirectories()
		{
			foreach (string childPath in System.IO.Directory.GetDirectories(FullName))
			{
				try
				{
					if (SelectChild(Path.GetFileName(childPath)) == null)
					{
						FileSystem.DeleteDirectory(childPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e}");
				}
			}
		}

		private void CleanupSubFiles()
		{
			foreach (string childPath in System.IO.Directory.GetFiles(FullName))
			{
				try
				{
					if (SelectChild(Path.GetFileName(childPath)) == null)
					{
						FileSystem.DeleteFile(childPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e}");
				}
			}
		}
	}
}
