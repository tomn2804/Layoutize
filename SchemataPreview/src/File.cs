using Microsoft.VisualBasic.FileIO;
using System;

namespace SchemataPreview.Models
{
	public partial class File : Model
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

		public File(string name)
			: base(name)
		{
		}
	}

	public partial class File : Model
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
