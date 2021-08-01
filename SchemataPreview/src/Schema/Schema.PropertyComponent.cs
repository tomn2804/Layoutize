namespace SchemataPreview
{
	public abstract partial class Schema
	{
		internal abstract class PropertyComponent<T> : Component
		{
			protected PropertyComponent(Schema model)
				: base(model)
			{
			}

			protected T Value { get; set; } = default;
		}
	}
}
