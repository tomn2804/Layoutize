using Microsoft.VisualBasic.FileIO;
using System;

namespace SchemataPreview
{
	public partial class FileModel : Model
	{
		public override bool Exists
		{
			get
			{
				if (Convert.ToBoolean(FullName))
				{
					return System.IO.File.Exists(FullName);
				}
				return false;
			}
		}

		public FileModel(string name)
			: base(name)
		{
		}
	}

	public partial class FileModel : Model
	{
		public override void Create() => System.IO.File.Create(FullName).Dispose();

		public override void Delete() => FileController.SendToRecycleBin(FullName);
	}

	public static class FileController
	{
		public static void SendToRecycleBin(string path)
		{
			FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
