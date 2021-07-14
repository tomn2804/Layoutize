using System;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		public override void PresetConfiguration()
		{
			base.PresetConfiguration();
			AddEventListener(EventOption.Create, () =>
			{
				Directory.CreateDirectory(FullName);
			});
			AddEventListener(EventOption.Delete, () =>
			{
				RecycleBin.SendDirectoryToRecycleBin(FullName);
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
								RecycleBin.SendDirectoryToRecycleBin(path);
							}
							else
							{
								RecycleBin.SendFileToRecycleBin(path);
							}
						}
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e}");
						}
					}
				}
			});
		}

		public override bool Exists { get => Directory.Exists(FullName); }
	}
}
