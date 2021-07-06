using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public partial class DirectoryModel : Model
	{
		public override bool Exists
		{
			get
			{
				if (Convert.ToBoolean(FullName))
				{
					return System.IO.Directory.Exists(FullName);
				}
				return false;
			}
		}

		public DirectoryModel(string name)
			: base(name)
		{
		}
	}

	public partial class DirectoryModel : Model
	{
		public override void Create() => System.IO.Directory.CreateDirectory(FullName);

		public override void Delete() => DirectoryController.SendToRecycleBin(FullName);

		public override void Cleanup() => Format();
	}

	public partial class DirectoryModel : Model
	{
		public void Format()
		{
			Format(System.IO.Directory.EnumerateDirectories(FullName), DirectoryController.SendToRecycleBin);
			Format(System.IO.Directory.EnumerateFiles(FullName), FileController.SendToRecycleBin);
		}

		private void Format(IEnumerable<string> paths, Action<string> handleDelete)
		{
			foreach (string path in paths)
			{
				try
				{
					if (SelectChild(Path.GetFileName(path)) == null)
					{
						handleDelete(path);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e}");
				}
			}
		}
	}

	public static class DirectoryController
	{
		public static void SendToRecycleBin(string path)
		{
			FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
		}
	}
}
