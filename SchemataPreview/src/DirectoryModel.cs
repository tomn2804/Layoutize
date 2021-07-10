using System;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : FileSystemModel
	{
		public DirectoryModel(string name)
			: base(name)
		{
			Configure(() =>
			{
				AddEventListener(EventOption.Create, () =>
				{
					Directory.CreateDirectory(FullName);
				});
				AddEventListener(EventOption.Delete, () =>
				{
					SendDirectoryToRecycleBin(FullName);
				});
				AddEventListener(EventOption.Cleanup, () =>
				{
					ForEachNonChild(Directory.EnumerateDirectories(FullName), SendDirectoryToRecycleBin);
					ForEachNonChild(Directory.EnumerateFiles(FullName), SendFileToRecycleBin);
				});
			});
		}

		public override bool Exists { get => Directory.Exists(FullName); }

		private void ForEachNonChild(IEnumerable<string> paths, Action<string> action)
		{
			foreach (string path in paths)
			{
				try
				{
					if (SelectChild(Path.GetFileName(path)) == null)
					{
						action(path);
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
