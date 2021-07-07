namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public ExcludeModel(string name)
			: base(name)
		{
		}

		public override bool Exists { get => false; }
	}
}
