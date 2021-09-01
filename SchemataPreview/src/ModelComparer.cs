using System.Collections.Generic;

namespace SchemataPreview
{
	public class ModelComparer : IComparer<Model>
	{
		public int Compare(Model? x, Model? y)
		{
			if (x != null)
			{
				if (y != null)
				{
					return y.Schema["Priority"]?.CompareTo(x.Schema["Priority"]) ?? x.Name.CompareTo(y.Name);
				}
				return 1;
			}
			return y == null ? 0 : -1;
		}
	}
}
