using System;

namespace SchemataPreview
{
	public abstract partial class Schema
	{
		internal class CachePropertyComponent<T> : PropertyComponent<T>
		{
			public CachePropertyComponent(Schema model, Func<T> initializer)
				: base(model)
			{
				Initializer = initializer;
			}

			private bool IsCached { get; set; }
			private Func<T> Initializer { get; set; }

			public new T Value
			{
				get
				{
					if (!IsCached)
					{
						base.Value = Initializer();
					}
					return base.Value;
				}
				set => base.Value = value;
			}

			public void ClearCache()
			{
				IsCached = false;
			}
		}
	}
}
