using Microsoft.VisualBasic.FileIO;

namespace SchemataPreview.Models
{
	public class Directory : Model
	{
		public Directory(string name)
			: base(name)
		{
		}

		public override void Create()
		{
			if (!IsMounted)
			{
				throw new ModelNotMountedException(this);
			}
			System.IO.Directory.CreateDirectory(FullName);
		}

		public override void Delete()
		{
			if (!IsMounted)
			{
				throw new ModelNotMountedException(this);
			}
			FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public override bool Exists()
		{
			return System.IO.Directory.Exists(FullName);
		}
	}
}
