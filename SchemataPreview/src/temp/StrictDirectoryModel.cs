
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace SchemataPreview
{
	public class StrictDirectoryModel : DirectoryModel
	{
		public StrictDirectoryModel(ImmutableDefinition Props)
			: base(props)
		{
			PipeAssembly.Register(PipeOption.Format).OnProcessing += (_, _) =>
			{
				foreach (string path in Directory.EnumerateFiles(FullName))
				{
					if (!Children.ContainsName(Path.GetFileName(path)))
					{
						try
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
						catch (Exception e)
						{
							Console.WriteLine($"Error: {e}");
						}
					}
				}
			};
		}
	}
}
