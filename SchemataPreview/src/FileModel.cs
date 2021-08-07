using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public override bool Exists => File.Exists(AbsolutePath);
		public override List<Model>? Children { get => null; protected internal set { } }

		public override void Create()
		{
			base.Create();
			File.Create(AbsolutePath).Dispose();
		}

		public override void Delete()
		{
			base.Delete();
			FileSystem.DeleteFile(AbsolutePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
