using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public FileModel(ReadOnlySchema schema)
			: base(schema)
		{
			PipeAssembly.Register(PipelineOption.Create).OnProcessing += () =>
			{
				File.Create(FullName).Dispose();
			});
			PipeAssembly.Register(PipelineOption.Delete).OnProcessing += () =>
			{
				FileSystem.DeleteFile(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			});
		}

		public override ModelSet? Children => null;
		public override bool Exists => File.Exists(FullName);
	}
}
