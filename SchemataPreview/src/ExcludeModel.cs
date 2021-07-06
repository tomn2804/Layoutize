namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public override bool Exists { get => false; }

		public ExcludeModel(string name)
			: base(name)
		{
		}
	}
}
