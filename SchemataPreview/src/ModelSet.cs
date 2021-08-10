#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace SchemataPreview
{
	public partial class ModelSet
	{
		public Model? this[string name] => Models.FirstOrDefault(model => model.Name == name);

		protected HashSet<Model> Models { get; } = new(new ModelComparer());

		public bool Add(params Model[] models)
		{
			bool result = true;
			foreach (Model model in models)
			{
				if (!Models.Add(model))
				{
					result = false;
				}
			}
			return result;
		}

		public bool Contains(string name)
		{
			return Models.Any(model => model.Name == name);
		}

		public bool Remove(string name)
		{
			return Convert.ToBoolean(Models.RemoveWhere(model => model.Name == name));
		}
	}
}

#nullable disable
