namespace SchemataPreview.Models
{
	public class Text : File
	{
		public Text(string name)
			: base(name)
		{
		}

		public new void Create()
		{
			base.Create();
			if (!IsMounted)
			{
				throw new ModelNotMountedException(this);
			}
			System.IO.File.Create(FullName).Dispose();
		}
	}
}
