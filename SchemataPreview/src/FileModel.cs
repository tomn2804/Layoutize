using System.IO;

namespace SchemataPreview
{
	public class FileModel : Model
	{
		public override void Build(Builder builder)
		{
			base.Build(builder);
			builder.AddEventListener(EventOption.Create, () =>
			{
				File.Create(FullName).Dispose();
			});
			builder.AddEventListener(EventOption.Delete, () =>
			{
				RecycleBin.DeleteFile(FullName);
			});
		}

		public override bool Exists { get => File.Exists(FullName); }
	}
}
