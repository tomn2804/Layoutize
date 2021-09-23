using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : FileSystemModel
	{
		public DirectoryModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly[PipeOption.Create].OnProcessing += (_, _) =>
			{
				Directory.CreateDirectory(FullName);
			};
			PipeAssembly[PipeOption.Delete].OnProcessing += (_, _) =>
			{
				FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			};
			if (schema.TryGetValue("Children", out object? children))
			{
				Children = new(this, (Schema[])children);
			}
			else
			{
				Children = new(this);
			}
		}

		public override ModelSet Children { get; }
		public override bool Exists => Directory.Exists(FullName);
	}
}
