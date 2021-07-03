namespace SchemataPreview.Models
{
	public class Exclude : Model
	{
		public Exclude(string name)
			: base(name)
		{
		}

		public override void Create()
		{
		}

		public override void Delete()
		{
		}

		public override bool Exists()
		{
			return false;
		}
	}
}
