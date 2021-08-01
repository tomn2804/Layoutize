namespace SchemataPreview
{
	public abstract partial class Schema
	{
		internal class InitializerPropertyComponent<T> : PropertyComponent<T>
		{
			public InitializerPropertyComponent(Schema model)
				: base(model)
			{
			}

			public new T Value
			{
				get => base.Value;
				set
				{
					if (Model.IsMounted)
					{
						throw;
					}
					base.Value = value;
				}
			}
		}
	}
}
