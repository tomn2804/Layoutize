using System;
using System.IO;

namespace SchemataPreview
{
	public class FileModel : FileSystemModel
	{
		public FileModel(string name)
			: base(name)
		{
		}

		public override bool Exists
		{
			get
			{
				if (Convert.ToBoolean(FullName))
				{
					return File.Exists(FullName);
				}
				return false;
			}
		}

		public override void Configure()
		{
			base.Configure();
			OnCreate(() => File.Create(FullName).Dispose());
			OnDelete(() => SendFileToRecycleBin(FullName));
		}
	}
}
