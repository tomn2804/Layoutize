namespace SchemataPreview.Models
{
	public class Exclude : Model
	{
		public override bool Exists { get => false; }

		public Exclude(string name)
			: base(name)
		{
		}
	}
}
