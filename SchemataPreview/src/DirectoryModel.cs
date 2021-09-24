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
			schema.TryGetValue("Children", out object? children);
			if (children != null)
			{
				Children = new(this, children.ToArray<Schema>());
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
