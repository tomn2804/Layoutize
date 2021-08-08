using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public class DirectoryModel : Model
	{
		public override bool Exists => Directory.Exists(AbsolutePath);
		public override List<Model> Children { get; } = new();

		public override void Create()
		{
			base.Create();
			Directory.CreateDirectory(AbsolutePath);
		}

		public override void Delete()
		{
			base.Delete();
			FileSystem.DeleteDirectory(AbsolutePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
