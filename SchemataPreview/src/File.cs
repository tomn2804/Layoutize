using Microsoft.VisualBasic.FileIO;

namespace SchemataPreview.Models
{
	public class File : Model
	{
		public File(string name)
			: base(name)
		{
		}

		public void Print()
		{
			System.Console.WriteLine(Name);
		}

		public override void Create()
		{
			if (!IsMounted)
			{
				throw new ModelNotMountedException(this);
			}
			System.IO.File.Create(FullName).Dispose();
		}

		public override void Delete()
		{
			if (!IsMounted)
			{
				throw new ModelNotMountedException(this);
			}
			FileSystem.DeleteFile(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}

		public override bool Exists()
		{
			return System.IO.File.Exists(FullName);
		}
	}
}
