using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
	public interface IDirectory
	{
	}

	public class DirectoryModel : Model
	{
		public DirectoryModel(ImmutableSchema schema)
			: base(schema)
		{
		}

		public override bool Exists => Directory.Exists(FullName);

		public override void Create()
		{
			Directory.CreateDirectory(FullName);
		}

		public override void Delete()
		{
			FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
