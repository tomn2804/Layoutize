using System.Collections.Generic;

namespace SchemataPreview
{
	public class ModelComparer : IComparer<Model>
	{
		public int Compare(Model? x, Model? y)
		{
			return x?.CompareTo(y) ?? y?.CompareTo(x) ?? 0;
		}
	}
}
