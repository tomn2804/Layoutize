using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace SchemataPreview
{
	public class StrictDirectoryModel : DirectoryModel
	{
		public virtual void Format()
		{
			foreach (string path in Directory.EnumerateFiles(FullName))
			{
				try
				{
					if (Children[Path.GetFileName(path)] is FileSystemModel child)
					{
						ModelBuilder.HandleDelete(child);
					}
					else
					{
						if (Directory.Exists(path))
						{
							FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
						}
						else
						{
							FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
						}
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
