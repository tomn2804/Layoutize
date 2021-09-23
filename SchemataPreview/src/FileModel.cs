using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : FileSystemModel
	{
		public FileModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly[PipelineOption.Create].OnProcessing += (_, _) =>
			{
				File.Create(FullName).Dispose();
			};
			PipeAssembly[PipelineOption.Delete].OnProcessing += (_, _) =>
			{
				FileSystem.DeleteFile(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			};
		}

		public override ModelSet? Children => null;
		public override bool Exists => File.Exists(FullName);
	}
}
