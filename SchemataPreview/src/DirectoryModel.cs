using System;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
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
					RecycleBin.RemoveDirectory(FullName);
				});
				AddEventListener(EventOption.Cleanup, () =>
				{
					foreach (string path in Directory.EnumerateFileSystemEntries(FullName))
					{
						if (SelectChild(Path.GetFileName(path)) == null)
						{
							try
							{
								if (Directory.Exists(path))
								{
									RecycleBin.RemoveDirectory(path);
								}
								else
								{
									RecycleBin.RemoveFile(path);
								}
							}
							catch (Exception e)
							{
								Console.WriteLine($"Error: {e}");
							}
						}
					}
				});
			});
		}

		public override bool Exists { get => Directory.Exists(FullName); }
	}
}
