using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		public DirectoryModel(ReadOnlySchema schema)
			: base(schema)
		{
			Children = new(this);
			PipeAssembly.Register(PipelineOption.Create).OnProcessing += () =>
			{
				Directory.CreateDirectory(FullName);
			});
			PipeAssembly.Register(PipelineOption.Delete).OnProcessing += () =>
			{
				FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			});
		}

		public override ModelSet Children { get; }
		public override bool Exists => Directory.Exists(FullName);
	}
}
