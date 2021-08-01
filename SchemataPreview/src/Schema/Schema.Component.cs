namespace SchemataPreview
{
	public abstract partial class Schema
	{
		public abstract class Component
		{
			protected internal Component(Schema model)
			{
				Model = model;
			}

			protected internal Schema Model { get; set; }
		}
	}
}
