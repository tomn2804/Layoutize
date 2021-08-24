using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SchemataPreview
{
	public partial class ModelSet
	{
		public Model? this[string name] => Models.FirstOrDefault(model => model.Name == name);

		protected SortedSet<Model> Models { get; } = new(new ModelComparer());

		public bool Add(Model model)
		{
			return Models.Add(model);
		}

		public void Add(params Model[] models)
		{
			foreach (Model model in models)
			{
				Add(model);
			}
		}

		public void AddOrReplace(params Model[] models)
		{
			foreach (Model model in models)
			{
				if (!Add(model))
				{
					Remove(model.Name);
					Add(model);
				}
			}
		}

		public void Clear()
		{
			Models.Clear();
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

	public partial class ModelSet : IEnumerable<Model>
	{
		public IEnumerator<Model> GetEnumerator()
		{
			return Models.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Models.GetEnumerator();
		}
	}
}
