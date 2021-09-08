using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public partial class ModelSet
		{
			public ModelSet(Model parent)
			{
				Parent = parent;
			}

			public Model Parent { get; init; }
			public Model? this[string name] => Models.FirstOrDefault(model => model.Equals(name));

			public void Add(params Schema[] schemata)
			{
				switch (Parent.Schema["Traversal"])
				{
					case "ReversePostOrder":
						foreach (Schema schema in schemata)
						{
							Model child = schema.NewModel();
							child.Parent = Parent;
							Models.Add(child);
							child.Build();
						}
						foreach (Model child in Models)
						{
							if (child.Children != null)
							{
								ModelBuilder.HandleMount(child.Children);
							}
						}
						break;

					case "ReversePreOrder":
					default:
						foreach (Schema schema in schemata)
						{
							Model child = schema.NewModel();
							child.Parent = Parent;
							Models.Add(child);
							ModelBuilder.HandleMount(child);
						}
						break;
				}
			}

			public bool Contains(Model model)
			{
				return Models.Contains(model);
			}

			public bool ContainsName(string name)
			{
				return Models.Any(model => model.Equals(name));
			}

			public void Mount()
			{
				Models.Clear();
				if (Parent.Schema["Children"] is object[] schemata)
				{
					Add(Array.ConvertAll(schemata, schema => (Schema)schema));
				}
			}

			public bool Remove(Model model)
			{
				ModelBuilder.HandleDelete(model);
				return Models.Remove(model);
			}

			public bool RemoveByName(string name)
			{
				return Remove(Models.First(model => model.Equals(name)));
			}

			protected SortedSet<Model> Models { get; } = new(new ModelComparer());
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
}
