using System.Collections.Generic;

#nullable enable

namespace SchemataPreview
{
	public class ModelComparer : IEqualityComparer<Model>
	{
		public bool Equals(Model? x, Model? y)
		{
			return x?.Name == y?.Name;
		}

		public int GetHashCode(Model obj)
		{
			return obj.Name.GetHashCode();
		}
	}
}

#nullable disable
