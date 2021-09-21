using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : FileSystemModel
	{
		public DirectoryModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly[PipelineOption.Create].OnProcessing += () =>
			{
				Directory.CreateDirectory(FullName);
			};
			PipeAssembly[PipelineOption.Delete].OnProcessing += () =>
			{
				FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			};
			if (Schema["Children"] is Schema[] children)
			{
				Children = new(this, children);
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
