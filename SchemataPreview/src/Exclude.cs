namespace SchemataPreview.Models
{
	public class Exclude : Model
	{
		public Exclude(string name)
			: base(name)
		{
		}

		public override bool Exists() => false;
	}
}
