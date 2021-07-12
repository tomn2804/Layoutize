using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
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
					RecycleBin.RemoveFile(FullName);
				});
			});
		}

		public override bool Exists { get => File.Exists(FullName); }
	}
}
