using System;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		public override void Build(Builder builder)
		{
			base.Build(builder);
			builder.AddEventListener(EventOption.Create, () =>
			{
				Directory.CreateDirectory(FullName);
			});
			builder.AddEventListener(EventOption.Delete, () =>
			{
				RecycleBin.DeleteDirectory(FullName);
			});
			builder.AddEventListener(EventOption.Cleanup, () =>
			{
				foreach (string path in Directory.EnumerateFileSystemEntries(FullName))
				{
					if (SelectChild(Path.GetFileName(path)) == null)
					{
						try
						{
							if (Directory.Exists(path))
							{
								RecycleBin.DeleteDirectory(path);
							}
							else
							{
								RecycleBin.DeleteFile(path);
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
