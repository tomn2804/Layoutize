#nullable enable

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
			return x.Schema["Index"]?.CompareTo(y.Schema["Index"]) ?? y.Schema["Index"]?.CompareTo(x.Schema["Index"]) ?? x.Name.CompareTo(y.Name);
		}
	}
}

#nullable disable
