using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public override void PresetConfiguration()
		{
			base.PresetConfiguration();
			AddEventListener(EventOption.Create, () =>
			{
				File.Create(FullName).Dispose();
			});
			AddEventListener(EventOption.Delete, () =>
			{
				RecycleBin.SendFileToRecycleBin(FullName);
			});
		}

		public override bool Exists { get => File.Exists(FullName); }
	}
}
