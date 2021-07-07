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
		}

		public override bool Exists
		{
			get
			{
				if (Convert.ToBoolean(FullName))
				{
					return Directory.Exists(FullName);
				}
				return false;
			}
		}

		public override void Configure()
		{
			base.Configure();
			OnCreate(() => Directory.CreateDirectory(FullName));
			OnDelete(() => SendDirectoryToRecycleBin(FullName));
			OnCleanup(() =>
			{
				ForEachNonChild(Directory.EnumerateDirectories(FullName), SendDirectoryToRecycleBin);
				ForEachNonChild(Directory.EnumerateFiles(FullName), SendFileToRecycleBin);
			});
		}

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
