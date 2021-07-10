using System.IO;

namespace SchemataPreview
{
	public class FileModel : FileSystemModel
	{
		public FileModel(string name)
			: base(name)
		{
			Configure(() =>
			{
				AddEventListener(EventOption.Create, () =>
				{
					File.Create(FullName).Dispose();
				});
				AddEventListener(EventOption.Delete, () =>
				{
					SendFileToRecycleBin(FullName);
				});
			});
		}

		public override bool Exists { get => File.Exists(FullName); }
	}
}
