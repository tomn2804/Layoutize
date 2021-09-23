using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : FileSystemModel
	{
		public FileModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly[PipeOption.Create].OnProcessing += (_, _) =>
			{
				File.Create(FullName).Dispose();
			};
			PipeAssembly[PipeOption.Delete].OnProcessing += (_, _) =>
			{
				FileSystem.DeleteFile(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			};
		}

		public override ModelSet? Children => null;
		public override bool Exists => File.Exists(FullName);
	}
}
