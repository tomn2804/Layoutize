namespace SchemataPreview
{
	public abstract partial class Schema
	{
		public class CacheComponent : Component
		{
			internal CacheComponent(Schema model)
				: base(model)
			{
			}

			public void Clear()
			{
				Model._fullName.ClearCache();
			}

			public void ClearAll()
			{
				Clear();
				foreach (Schema child in Model.Children)
				{
					child.Cache.ClearAll();
				}
			}
		}
	}
}
