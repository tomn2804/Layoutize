using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : FileSystemModel
	{
		public FileModel(ReadOnlySchema schema)
			: base(schema)
		{
			PipeAssembly[PipelineOption.Create].OnProcessing += () =>
			{
				File.Create(FullName).Dispose();
			});
			PipeAssembly[PipelineOption.Delete].OnProcessing += () =>
			{
				FileSystem.DeleteFile(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			});
		}

		public override ModelSet? Children => null;
		public override bool Exists => File.Exists(FullName);
	}
}
