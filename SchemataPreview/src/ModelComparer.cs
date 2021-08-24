using System.Collections.Generic;

namespace SchemataPreview
{
	public class ModelComparer : IComparer<Model>
	{
		public int Compare(Model? x, Model? y)
		{
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return y.Schema["Priority"]?.CompareTo(x.Schema["Priority"]) ?? x.Name.CompareTo(y.Name);
		}
	}
}
